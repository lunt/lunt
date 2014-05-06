using System;
using Lunt.IO;

namespace Lunt
{
    /// <summary>
    /// Represent a dependency to an asset.
    /// </summary>
    public sealed class AssetDependency
    {
        private readonly FilePath _path;
        private readonly long _fileSize;
        private readonly string _checksum;

        /// <summary>
        /// Gets the path of the dependency.
        /// </summary>
        /// <value>The path of the dependency.</value>
        public FilePath Path
        {
            get { return _path; }
        }

        /// <summary>
        /// Gets the file size of the dependency.
        /// </summary>
        /// <value>The file size of the dependency.</value>
        public long FileSize
        {
            get { return _fileSize; }
        }

        /// <summary>
        /// Gets the checksum of the dependency.
        /// </summary>
        /// <value>The checksum of the dependency.</value>
        public string Checksum
        {
            get { return _checksum; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetDependency" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="fileSize">The file size.</param>
        /// <param name="checksum">The checksum.</param>
        public AssetDependency(FilePath path, long fileSize, string checksum)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (checksum == null)
            {
                throw new ArgumentNullException("checksum");
            }
            _path = path;
            _fileSize = fileSize;
            _checksum = checksum;
        }
    }
}