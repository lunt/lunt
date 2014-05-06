using System;
using System.Collections.Generic;
using Lunt.IO;

namespace Lunt
{
    /// <summary>
    /// Represents a yet to be resolved asset.
    /// </summary>
    public sealed class AssetDefinition 
    {
        private readonly FilePath _path;
        private readonly bool _isGlob;
        private readonly IDictionary<string, string> _metadata;

        /// <summary>
        /// Gets the path to the asset.
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
        public IDictionary<string, string> Metadata
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
        /// Gets a value indicating whether the path is a glob pattern.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the path is a glob pattern; otherwise, <c>false</c>.
        /// </value>
        public bool IsGlob
        {
            get { return _isGlob; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetDefinition"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="metadata">The metadata.</param>
        public AssetDefinition(FilePath path, IDictionary<string, string> metadata = null)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            _path = path;
            _metadata = metadata ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            _isGlob = path.FullPath.Contains("*") || path.FullPath.Contains("?");
        }
    }
}
