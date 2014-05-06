using System;
using IOFile = System.IO.FileInfo;
using IODirectory = System.IO.DirectoryInfo;

namespace Lunt.IO
{
    /// <summary>
    /// Representation of a physical file system.
    /// </summary>
    public sealed class FileSystem : IFileSystem
    {
        /// <summary>
        /// Gets a value indicating whether the file system is case sensitive.
        /// </summary>
        /// <value>
        /// <c>true</c> if the file system is case sensitive; otherwise, <c>false</c>.
        /// </value>
        public bool IsCaseSensitive
        {
            get
            {
                var platform = (int) Environment.OSVersion.Platform;
                var isUnix = (platform == 4) || (platform == 6) || (platform == 128);
                return isUnix;
            }
        }

        /// <summary>
        /// Gets a file representation of the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// A file representation.
        /// </returns>
        public IFile GetFile(FilePath path)
        {
            return new File(new IOFile(path.FullPath));
        }

        /// <summary>
        /// Gets a directory representation of the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// A directory representation.
        /// </returns>
        public IDirectory GetDirectory(DirectoryPath path)
        {
            return new Directory(new IODirectory(path.FullPath));
        }
    }
}