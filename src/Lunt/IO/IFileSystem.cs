namespace Lunt.IO
{
    /// <summary>
    /// File system abstraction.
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// Gets a value indicating whether the file system is case sensitive.
        /// </summary>
        /// <value>
        /// <c>true</c> if the file system is case sensitive; otherwise, <c>false</c>.
        /// </value>
        bool IsCaseSensitive { get; }

        /// <summary>
        /// Gets a file representation of the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A file representation.</returns>
        IFile GetFile(FilePath path);

        /// <summary>
        /// Gets a directory representation of the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A directory representation.</returns>
        IDirectory GetDirectory(DirectoryPath path);
    }
}