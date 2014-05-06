using Lunt.IO;

namespace Lunt
{
    /// <summary>
    /// Provides information about the current environment and platform.
    /// </summary>
    public interface IBuildEnvironment
    {
        /// <summary>
        /// Gets the file system.
        /// </summary>
        /// <value>The file system.</value>
        IFileSystem FileSystem { get; }

        /// <summary>
        /// Determines whether the operative system is Unix based.
        /// </summary>
        /// <returns><c>true</c> if operative system is Unix based; otherwise, <c>false</c>.</returns>
        bool IsUnix();

        /// <summary>
        /// Gets the fully qualified path of the current working directory.
        /// </summary>
        /// <returns>The working directory path.</returns>
        DirectoryPath GetWorkingDirectory();
    }
}