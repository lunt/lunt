using System.IO;

namespace Lunt.IO
{
    /// <summary>
    /// A representation of a file.
    /// </summary>
    public interface IFile
    {
        /// <summary>
        /// Gets the file path.
        /// </summary>
        /// <value>The path.</value>
        FilePath Path { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IFile" /> exists.
        /// </summary>
        /// <value>
        ///   <c>true</c> if exists; otherwise, <c>false</c>.
        /// </value>
        bool Exists { get; }

        /// <summary>
        /// Gets the file size in bytes.
        /// </summary>
        /// <value>The file size in bytes.</value>
        long Length { get; }

        /// <summary>
        /// Opens a file in the specified mode with read, write, or read/write access and the specified sharing option.
        /// </summary>
        /// <param name="fileMode">The file mode.</param>
        /// <param name="fileAccess">The file access.</param>
        /// <param name="fileShare">The file share.</param>
        /// <returns></returns>
        Stream Open(FileMode fileMode, FileAccess fileAccess, FileShare fileShare);
    }
}