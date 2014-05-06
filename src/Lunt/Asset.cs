using System;
using System.Collections.Generic;
using Lunt.IO;

namespace Lunt
{
    /// <summary>
    /// Represents a resolved asset.
    /// </summary>
    public sealed class Asset
    {
        private readonly FilePath _path;
        private readonly AssetMetadata _metadata;

        /// <summary>
        /// Gets the relative path to the asset.
        /// </summary>
        /// <value>The path.</value>
        public FilePath Path
        {
            get { return _path; }
        }

        /// <summary>
        /// Gets the asset metadata.
        /// </summary>
        /// <value>The metadata.</value>
        public AssetMetadata Metadata
        {
            get { return _metadata; }
        }

        /// <summary>
        /// Gets or sets the name of the processor to be used when building the asset.
        /// </summary>
        /// <value>
        /// The name of the processor to be used when building the asset.
        /// </value>
        public string ProcessorName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Asset" /> class.
        /// </summary>
        /// <param name="path">The relative path to the asset.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">path</exception>
        /// <exception cref="System.ArgumentNullException">metadata</exception>
        public Asset(FilePath path, IDictionary<string, string> metadata = null)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            _path = path;
            _metadata = new AssetMetadata(metadata ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets the source file path.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public FilePath GetSourceFilePath(BuildConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }
            return configuration.InputDirectory.Combine(_path);
        }

        /// <summary>
        /// Gets the target file path.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public FilePath GetTargetFilePath(BuildConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }
            return configuration.OutputDirectory.Combine(_path.ChangeExtension(".dat"));
        }
    }
}