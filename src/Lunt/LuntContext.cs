using System;
using System.Collections.Generic;
using System.Globalization;
using Lunt.Diagnostics;
using Lunt.IO;

namespace Lunt
{
    /// <summary>
    /// Provides a context used by components during build.
    /// </summary>
    public sealed class LuntContext
    {
        private readonly IFileSystem _fileSystem;
        private readonly BuildConfiguration _configuration;
        private readonly IBuildLog _log;
        private readonly Asset _asset;
        private readonly IHashComputer _hasher;
        private readonly List<AssetDependency> _dependencies;
        private readonly HashSet<FilePath> _dependencyPaths;

        /// <summary>
        /// Gets the file system representation.
        /// </summary>
        /// <value>The file system representation.</value>
        public IFileSystem FileSystem
        {
            get { return _fileSystem; }
        }

        /// <summary>
        /// Gets the build log.
        /// </summary>
        /// <value>The build log.</value>
        public IBuildLog Log
        {
            get { return _log; }
        }

        /// <summary>
        /// Gets the asset.
        /// </summary>
        /// <value>The asset.</value>
        public Asset Asset
        {
            get { return _asset; }
        }

        /// <summary>
        /// Gets the input directory path.
        /// </summary>
        /// <value>The input directory path.</value>
        public DirectoryPath InputDirectory
        {
            get { return new DirectoryPath(_configuration.InputDirectory.FullPath); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LuntContext" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="configuration">The build configuration.</param>
        /// <param name="hasher">The hash computer.</param>
        /// <param name="log">The log.</param>
        /// <param name="asset">The asset.</param>
        public LuntContext(IFileSystem fileSystem, BuildConfiguration configuration, IHashComputer hasher, IBuildLog log, Asset asset)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }
            if (hasher == null)
            {
                throw new ArgumentNullException("hasher");
            }
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }
            if (asset == null)
            {
                throw new ArgumentNullException("asset");
            }
            _fileSystem = fileSystem;
            _configuration = configuration;
            _hasher = hasher;
            _log = log;
            _asset = asset;
            _dependencies = new List<AssetDependency>();
            _dependencyPaths = new HashSet<FilePath>(new PathComparer(_fileSystem.IsCaseSensitive));
        }

        /// <summary>
        /// Adds a dependency to the asset being currently built.
        /// </summary>
        /// <param name="file">The dependency to be added.</param>
        public void AddDependency(IFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }

            if (!file.Exists)
            {
                string message = string.Format(CultureInfo.InvariantCulture, "The dependency '{0}' does not exist.", file.Path);
                throw new LuntException(message);
            }

            // Must be part of the input directory.
            if (!file.Path.FullPath.StartsWith(_configuration.InputDirectory.FullPath))
            {
                string message = string.Format(CultureInfo.InvariantCulture, "The dependency '{0}' is not relative to input directory.", file.Path);
                throw new LuntException(message);
            }

            // Get the relative path.
            var path = new FilePath(file.Path.FullPath.Substring(_configuration.InputDirectory.FullPath.Length + 1));
            if (!_dependencyPaths.Contains(path))
            {
                var fileSize = file.Length;
                var checksum = _hasher.Compute(file);
                var dependency = new AssetDependency(path, fileSize, checksum);

                _dependencies.Add(dependency);
                _dependencyPaths.Add(path);
            }
        }

        /// <summary>
        /// Gets all dependencies associated with the asset being currently built.
        /// </summary>
        /// <returns>The dependencies associated with the asset.</returns>
        public AssetDependency[] GetDependencies()
        {
            return _dependencies.ToArray();
        }
    }
}