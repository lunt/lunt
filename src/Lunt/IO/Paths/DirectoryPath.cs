using System;
using System.ComponentModel;

namespace Lunt.IO
{
    /// <summary>
    /// Represents a directory path.
    /// </summary>
    [TypeConverter(typeof(DirectoryPathTypeConverter))]
    public sealed class DirectoryPath : Path
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryPath" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public DirectoryPath(string path)
            : base(path)
        {
        }

        /// <summary>
        /// Combines the current directory path with a file path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The combined file path.</returns>
        public FilePath Combine(FilePath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (!path.IsRelative)
            {
                throw new InvalidOperationException("Cannot combine a directory path with an absolute file path.");
            }
            var combinedPath = System.IO.Path.Combine(FullPath, path.FullPath);
            return new FilePath(combinedPath);
        }

        /// <summary>
        /// Combines the current directory path with another (relative) directory path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The combined directory path.</returns>
        public DirectoryPath Combine(DirectoryPath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (!path.IsRelative)
            {
                throw new InvalidOperationException("Cannot combine a directory path with an absolute directory path.");
            }
            var combinedPath = System.IO.Path.Combine(FullPath, path.FullPath);
            return new DirectoryPath(combinedPath);
        }

        /// <summary>
        /// Implicitly converts a string path to a directory path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A directory path.</returns>
        public static implicit operator DirectoryPath(string path)
        {
            return FromString(path);
        }

        /// <summary>
        /// Converts a string path to a directory path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static DirectoryPath FromString(string path)
        {
            return new DirectoryPath(path);
        }
    }
}