namespace Lunt.IO
{
    /// <summary>
    /// Representation of a physical file system.
    /// </summary>
    public sealed class FileSystem : IFileSystem
    {
        private readonly bool _isCaseSensitive;

        /// <summary>
        /// Gets a value indicating whether the file system is case sensitive.
        /// </summary>
        /// <value>
        /// <c>true</c> if the file system is case sensitive; otherwise, <c>false</c>.
        /// </value>
        public bool IsCaseSensitive
        {
            get { return _isCaseSensitive; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystem"/> class.
        /// </summary>
        public FileSystem()
        {
            _isCaseSensitive = Machine.IsUnix();
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
            return new File(path);
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
            return new Directory(path);
        }
    }
}