using System;
using System.Diagnostics.CodeAnalysis;

namespace Lunt
{
    /// <summary>
    /// The build manifest item.
    /// </summary>
    public sealed class BuildManifestItem
    {
        private readonly Asset _asset;
        private AssetDependency[] _dependencies;

        /// <summary>
        /// Gets the asset.
        /// </summary>
        /// <value>The asset.</value>
        public Asset Asset
        {
            get { return _asset; }
        }

        /// <summary>
        /// Gets or sets the build status.
        /// </summary>
        /// <value>The build status.</value>
        public AssetBuildStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the build message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the file length.
        /// </summary>
        /// <value>The length.</value>
        public long Length { get; set; }

        /// <summary>
        /// Gets or sets the file checksum.
        /// </summary>
        /// <value>The checksum.</value>
        public string Checksum { get; set; }

        /// <summary>
        /// Gets or sets the asset dependencies.
        /// </summary>
        /// <value>
        /// The asset dependencies.
        /// </value>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public AssetDependency[] Dependencies
        {
            get { return _dependencies; }
            set { _dependencies = value ?? new AssetDependency[] {}; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildManifestItem" /> class.
        /// </summary>
        /// <param name="asset">The asset.</param>
        /// <exception cref="System.ArgumentNullException">asset</exception>
        public BuildManifestItem(Asset asset)
        {
            if (asset == null)
            {
                throw new ArgumentNullException("asset");
            }
            _asset = asset;
            Status = AssetBuildStatus.Unknown;
            Message = string.Empty;
            Checksum = string.Empty;
            Length = 0;
            _dependencies = new AssetDependency[] {};
        }
    }
}