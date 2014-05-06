using System.ComponentModel;

namespace Lunt.IO
{
    /// <summary>
    /// Represent a file path.
    /// </summary>
    [TypeConverter(typeof (FilePathTypeConverter))]
    public sealed class FilePath : Path
    {
        /// <summary>
        /// Gets a value indicating whether the path has an extension.
        /// </summary>
        /// <value>
        /// <c>true</c> if the path has an extension; otherwise, <c>false</c>.
        /// </value>
        public bool HasExtension
        {
            get { return System.IO.Path.HasExtension(FullPath); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilePath" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public FilePath(string path)
            : base(path)
        {
        }

        /// <summary>
        /// Gets the directory part of the path.
        /// </summary>
        /// <returns>The directory-</returns>
        public DirectoryPath GetDirectory()
        {
            var directory = System.IO.Path.GetDirectoryName(FullPath);
            if (string.IsNullOrWhiteSpace(directory))
            {
                directory = "./";
            }
            return new DirectoryPath(directory);
        }

        /// <summary>
        /// Gets the filename part of the path.
        /// </summary>
        /// <returns>The filename.</returns>
        public FilePath GetFilename()
        {
            var filename = System.IO.Path.GetFileName(FullPath);
            return new FilePath(filename);
        }

        /// <summary>
        /// Gets the extension part of the path.
        /// </summary>
        /// <returns>The extension.</returns>
        public string GetExtension()
        {
            string extension = System.IO.Path.GetExtension(FullPath);
            return string.IsNullOrWhiteSpace(extension) ? null : extension;
        }

        /// <summary>
        /// Changes the extension of the path.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns>The path.</returns>
        public FilePath ChangeExtension(string extension)
        {
            return new FilePath(System.IO.Path.ChangeExtension(FullPath, extension));
        }

        /// <summary>
        /// Implicitly converts a string to a file path.
        /// </summary>
        /// <param name="path">The path string.</param>
        /// <returns>The file path</returns>
        public static implicit operator FilePath(string path)
        {
            return FromString(path);
        }

        /// <summary>
        /// Converts a string path to a file path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static FilePath FromString(string path)
        {
            return new FilePath(path);
        }
    }
}