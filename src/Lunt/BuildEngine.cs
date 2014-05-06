using System;
using System.Collections.Generic;
using System.Linq;
using Lunt.Diagnostics;
using Lunt.IO;

namespace Lunt
{
    /// <summary>
    /// The Lunt build engine.
    /// </summary>
    public sealed class BuildEngine : IBuildEngine, IDisposable
    {
        private readonly IFileSystem _fileSystem;
        private readonly AssetBuilder _builder;
        private readonly IBuildLog _log;
        private readonly AssetExtractor _extractor;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildEngine" /> class.
        /// </summary>
        /// <param name="environment">The build environment</param>
        /// <param name="components">The components.</param>
        /// <param name="hasher">The hash computer.</param>
        /// <param name="log">The log.</param>
        /// <exception cref="System.ArgumentNullException">fileSystem</exception>
        public BuildEngine(IBuildEnvironment environment, IPipelineComponentCollection components,
            IHashComputer hasher, IBuildLog log)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
            if (environment.FileSystem == null)
            {
                throw new ArgumentException("The build environment's file system was null.", "environment");
            }
            if (components == null)
            {
                throw new ArgumentNullException("components");
            }
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }

            _fileSystem = environment.FileSystem;
            _builder = new AssetBuilder(_fileSystem, components, hasher, log);
            _log = log;
            _extractor = new AssetExtractor(environment);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _builder.Dispose();
                _disposed = true;
            }
        }

        /// <summary>
        /// Performs a build using the specified build configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The build manifest.</returns>
        public BuildManifest Build(BuildConfiguration configuration)
        {
            return Build(configuration, null);
        }

        /// <summary>
        /// Performs a build using the specified build configuration and manifest.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="previousManifest">The previous manifest.</param>
        /// <returns>The build manifest.</returns>
        /// <exception cref="System.ArgumentNullException">configuration</exception>
        public BuildManifest Build(BuildConfiguration configuration, BuildManifest previousManifest)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName, "The build engine has been disposed.");
            }

            // Validate the configuration.
            BuildConfigurationValidator.Validate(_fileSystem, configuration);

            // Collapse all paths since we know that they're not relative.
            configuration.InputDirectory = new DirectoryPath(PathNormalizer.Collapse(configuration.InputDirectory));
            configuration.OutputDirectory = new DirectoryPath(PathNormalizer.Collapse(configuration.OutputDirectory));

            // No assets provided?
            if (configuration.Assets.Count == 0)
            {
                // Return an empty manifest.
                return new BuildManifest();
            }

            // Create an empty manifest.
            var manifest = new BuildManifest();

            // Extract all assets.
            var assets = _extractor.Extract(configuration);

            // Build all assets.
            foreach (var asset in assets)
            {
                if (previousManifest != null)
                {
                    // Find the manifest.
                    var assetManifest = previousManifest.Items.FirstOrDefault(x => x.Asset.Path.FullPath == asset.Path.FullPath);
                    if (assetManifest != null)
                    {
                        if (!ShouldRebuild(configuration, asset, assetManifest))
                        {
                            _log.Information("Skipped @{0} (no change)", asset.Path);
                            assetManifest.Status = AssetBuildStatus.Skipped;
                            manifest.Items.Add(assetManifest);
                            continue;
                        }
                    }
                }

                _log.Information("Building @{0}", asset.Path);

                // Build the item.
                var result = _builder.Build(asset, configuration);
                if (result.Status != AssetBuildStatus.Success)
                {
                    _log.Warning(result.Message);
                }

                // Add the result to the manifest.
                manifest.Items.Add(result.ManifestItem);
            }

            // Return the manifest.
            return manifest;
        }

        private bool ShouldRebuild(BuildConfiguration configuration, Asset asset, BuildManifestItem manifest)
        {
            // No incremental build?
            if (!configuration.Incremental)
            {
                return true;
            }

            // Rebuild if the target file does not exist.
            var targetFile = _fileSystem.GetFile(asset.GetTargetFilePath(configuration));
            if (!targetFile.Exists)
            {
                _log.Verbose("Target file does not exist. Rebuilding asset.");
                return true;
            }

            // Get the source file.
            var sourceFile = _fileSystem.GetFile(asset.GetSourceFilePath(configuration));

            // Rebuild if the file sizes don't match.
            if (sourceFile.Length != manifest.Length)
            {
                _log.Verbose("Source file size have changed. Rebuilding asset.");
                return true;
            }

            // Rebuild if the file hash have changed.
            var hash = _builder.Hasher.Compute(sourceFile);
            if (!hash.Equals(manifest.Checksum, StringComparison.Ordinal))
            {
                _log.Verbose("Source file checksum have changed. Rebuilding asset.");
                return true;
            }

            var manifestKeys = new HashSet<string>(manifest.Asset.Metadata.GetKeys());
            var assetKeys = new HashSet<string>(asset.Metadata.GetKeys());

            // Rebuild if the meta data count have changed.
            if (manifestKeys.Count != assetKeys.Count)
            {
                _log.Verbose("Asset metadata have changed. Rebuilding asset.");
                return true;
            }

            // Is there a symmetric difference in the keys?
            var added = manifestKeys.Except(assetKeys).ToArray();
            var removed = assetKeys.Except(manifestKeys).ToArray();
            var diff = new HashSet<string>(added.Concat(removed));
            if (diff.Count > 0)
            {
                _log.Verbose("Asset metadata have changed. Rebuilding asset.");
                return true;
            }

            // Is there a change in values?
            foreach (var assetKey in assetKeys)
            {
                var assetValue = asset.Metadata.GetValue(assetKey);
                var manifestValue = manifest.Asset.Metadata.GetValue(assetKey);
                if (!assetValue.Equals(manifestValue, StringComparison.Ordinal))
                {
                    _log.Verbose("Asset metadata have changed. Rebuilding asset.");
                    return true;
                }
            }

            // Has dependencies?
            if (manifest.Dependencies != null)
            {
                foreach (var dependency in manifest.Dependencies)
                {
                    var filePath = configuration.InputDirectory.Combine(dependency.Path);
                    var file = _fileSystem.GetFile(filePath);
                    if (!file.Exists)
                    {
                        _log.Verbose("The dependency '{0}' has been removed. Rebuilding asset.", dependency.Path);
                        return true;
                    }

                    if (file.Length != dependency.FileSize)
                    {
                        _log.Verbose("The file size of dependency '{0}' has changed. Rebuilding asset.", dependency.Path);
                        return true;
                    }

                    var dependencyHash = _builder.Hasher.Compute(file);
                    if (!dependencyHash.Equals(dependency.Checksum, StringComparison.Ordinal))
                    {
                        _log.Verbose("The checksum of dependency '{0}' has changed. Rebuilding asset.", dependency.Path);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}