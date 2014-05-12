using System;
using Lunt.Descriptors;
using Lunt.Diagnostics;
using Lunt.IO;
using Lunt.Runtime;

namespace Lunt
{
    internal sealed class AssetBuilder : IDisposable
    {
        private readonly IFileSystem _fileSystem;
        private readonly DescriptorRegistry _registry;
        private readonly IBuildLog _log;
        private readonly IHashComputer _hasher;
        private bool _disposed;

        public IHashComputer Hasher
        {
            get { return _hasher; }
        }

        public AssetBuilder(IFileSystem fileSystem, IPipelineScanner scanner, IHashComputer hasher, IBuildLog log)
        {
            _fileSystem = fileSystem;
            _registry = new DescriptorRegistry(scanner.Scan());
            _hasher = hasher;
            _log = log;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _hasher.Dispose();
                _disposed = true;
            }
        }

        public AssetBuildResult Build(Asset asset, BuildConfiguration configuration)
        {
            object importedData;

            var result = BuildAsset(asset, configuration, out importedData);
            if (importedData != null)
            {
                var disposable = importedData as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }

            return result;
        }

        private AssetBuildResult BuildAsset(Asset asset, BuildConfiguration configuration, out object obj)
        {
            obj = null;

            // Create the manifest for the asset.
            var context = new Context(_fileSystem, configuration, _hasher, _log, asset);
            var manifest = new BuildManifestItem(asset);

            // Makes ure the asset has a file extension.
            var extension = asset.Path.GetExtension();
            if (extension == null)
            {
                var message = new Message("The asset {0} has no file extension.", asset.Path);
                return AssetBuildResult.Failure(manifest, message);
            }

            // Get the source file and make sure it exist.
            var sourcePath = asset.GetSourceFilePath(configuration);
            var sourceFile = _fileSystem.GetFile(sourcePath);
            if (!sourceFile.Exists)
            {
                var message = new Message("Could not find the file '{0}'.", sourcePath.FullPath);
                return AssetBuildResult.Failure(manifest, message);
            }

            AssetBuildResult result;
            if (!BuildAsset(asset, configuration, manifest, context, sourceFile, ref obj, out result))
            {
                return result;
            }

            _log.Information("Built {0}", asset.Path);

            manifest.Checksum = _hasher.Compute(sourceFile);
            manifest.Length = sourceFile.Length;
            manifest.Dependencies = context.GetDependencies();

            return AssetBuildResult.Success(manifest);
        }

        private bool BuildAsset(Asset asset, BuildConfiguration configuration, BuildManifestItem manifest,
            Context context, IFile sourceFile, ref object obj, out AssetBuildResult result)
        {
            if (!ImportAsset(asset, manifest, context, sourceFile, ref obj, out result))
            {
                return false;
            }
            if (!ProcessAsset(asset, manifest, context, ref obj, out result))
            {
                return false;
            }
            if (!WriteAsset(asset, configuration, manifest, context, obj, out result))
            {
                return false;
            }
            return true;
        }

        private bool ImportAsset(Asset asset, BuildManifestItem manifest, Context context,
            IFile sourceFile, ref object obj, out AssetBuildResult error)
        {
            // Get the importer from the registry.
            var importerDescription = _registry.GetImporter(asset);
            if (importerDescription == null)
            {
                var message = new Message("Could not find an importer for {0}.", asset.Path);
                error = AssetBuildResult.Failure(manifest, message);
                return false;
            }

            _log.Verbose("{0}: Importing {1}", importerDescription.Name, asset.Path);
            obj = importerDescription.Importer.Import(context, sourceFile);
            if (obj == null)
            {
                var message = new Message("Import of {0} resulted in null.", asset.Path);
                error = AssetBuildResult.Failure(manifest, message);
                return false;
            }

            error = null;
            return true;
        }

        private bool ProcessAsset(Asset asset, BuildManifestItem manifest,
            Context context, ref object obj, out AssetBuildResult error)
        {
            // Get the processor (if any) from the registry.
            var processorDescription = _registry.GetProcessor(asset);
            if (processorDescription != null)
            {
                // Make sure the obj type and the processor source type match.
                // We do this since it's possible to directly implement 
                // the ILuntImporter and return an object of a type that's 
                // inconsistent with the type that the importer claims it handle.
                var importedType = obj.GetType();
                var processorSourceType = processorDescription.SourceType;
                if (importedType != processorSourceType)
                {
                    const string format = "Cannot process {0} since the data is of the wrong type ({1}). Processor expected {2}.";
                    var message = new Message(format, asset.Path, importedType.FullName, processorSourceType.FullName);
                    error = AssetBuildResult.Failure(manifest, message);
                    return false;
                }

                _log.Verbose("{0}: Processing {1}", processorDescription.Name, asset.Path);
                obj = processorDescription.Processor.Process(context, obj);
                if (obj == null)
                {
                    var message = new Message("Processing of {0} resulted in null.", asset.Path);
                    error = AssetBuildResult.Failure(manifest, message);
                    return false;
                }

                if (obj.GetType() != processorDescription.TargetType)
                {
                    const string format = "Value returned from processor for {0} does not match expected target type ({1}).";
                    var message = new Message(format, asset.Path, processorDescription.TargetType.FullName);
                    error = AssetBuildResult.Failure(manifest, message);
                    return false;
                }
            }

            if (!string.IsNullOrWhiteSpace(asset.ProcessorName) && processorDescription == null)
            {
                const string format = "Cannot process {0} since the processor '{1}' wasn't found.";
                var message = new Message(format, asset.Path, asset.ProcessorName);
                error = AssetBuildResult.Failure(manifest, message);
                return false;
            }

            error = null;
            return true;
        }

        private bool WriteAsset(Asset asset, BuildConfiguration configuration, BuildManifestItem manifest,
            Context context, object obj, out AssetBuildResult error)
        {
            // Get the writer from the registry.
            var writerDescription = _registry.GetWriter(obj.GetType());
            if (writerDescription == null)
            {
                var message = new Message("Could not find a writer for {0}.", asset.Path);
                error = AssetBuildResult.Failure(manifest, message);
                return false;
            }

            // Get the target file.
            var targetPath = asset.GetTargetFilePath(configuration);
            var targetFile = _fileSystem.GetFile(targetPath);

            // Does the target file directory path exist?
            var targetDirectory = _fileSystem.GetDirectory(targetPath.GetDirectory());
            if (!targetDirectory.Exists)
            {
                if (!targetDirectory.Create())
                {
                    const string format = "Could not create target directory '{0}'.";
                    var message = new Message(format, targetDirectory.Path);
                    error = AssetBuildResult.Failure(manifest, message);
                    return false;
                }
            }

            _log.Verbose("{0}: Writing {1}", writerDescription.Name, asset.Path);
            writerDescription.Writer.Write(context, targetFile, obj);

            error = null;
            return true;
        }
    }
}