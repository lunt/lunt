using System.Collections.Generic;
using Lunt.IO;

namespace Lunt
{
    /// <summary>
    /// Represents a build configuration.
    /// </summary>
    public sealed class BuildConfiguration
    {
        private readonly List<AssetDefinition> _assets;

        /// <summary>
        /// Gets or sets the input directory.
        /// </summary>
        /// <value>The input directory.</value>
        public DirectoryPath InputDirectory { get; set; }

        /// <summary>
        /// Gets or sets the output directory.
        /// </summary>
        /// <value>The output directory.</value>
        public DirectoryPath OutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a build is incremental.
        /// </summary>
        /// <value>
        ///   <c>true</c> if incremental; otherwise, <c>false</c>.
        /// </value>
        public bool Incremental { get; set; }

        /// <summary>
        /// Gets the assets collection.
        /// </summary>
        /// <value>The assets collection.</value>
        public IList<AssetDefinition> Assets
        {
            get { return _assets; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildConfiguration" /> class.
        /// </summary>
        public BuildConfiguration()
        {
            _assets = new List<AssetDefinition>();
        }
    }
}