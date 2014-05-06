using System.IO;

namespace Lunt.IO
{
    /// <summary>
    /// Representation of a physical file.
    /// </summary>
    public sealed class File : IFile
    {
        private readonly FileInfo _file;
        private readonly FilePath _path;

        /// <summary>
        /// Gets the file path.
        /// </summary>
        /// <value>The path.</value>
        public FilePath Path
        {
            get { return _path; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IFile" /> exists.
        /// </summary>
        /// <value>
        ///   <c>true</c> if exists; otherwise, <c>false</c>.
        /// </value>
        public bool Exists
        {
            get { return _file.Exists; }
        }

        /// <summary>
        /// Gets the file size in bytes.
        /// </summary>
        /// <value>The file size in bytes.</value>
        public long Length
        {
            get { return _file.Length; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="File" /> class.
        /// </summary>
        /// <param name="file">The file.</param>
        public File(FileInfo file)
        {
            _file = file;
            _path = new FilePath(file.FullName);
        }

        /// <summary>
        /// Opens a file in the specified mode with read, write, or read/write access and the specified sharing option.
        /// </summary>
        /// <param name="fileMode">The file mode.</param>
        /// <param name="fileAccess">The file access.</param>
        /// <param name="fileShare">The file share.</param>
        /// <returns>A stream to the file.</returns>
        public Stream Open(FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            return _file.Open(fileMode, fileAccess, fileShare);
        }
    }
}