using System;
using Lunt.IO;

namespace Lunt
{
    /// <summary>
    /// Provides information about the current environment and platform.
    /// </summary>
    public sealed class BuildEnvironment : IBuildEnvironment
    {
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// Gets the file system.
        /// </summary>
        /// <value>The file system.</value>
        public IFileSystem FileSystem
        {
            get { return _fileSystem; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildEnvironment"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        public BuildEnvironment(IFileSystem fileSystem)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }
            _fileSystem = fileSystem;
        }

        /// <summary>
        /// Determines whether the operative system is Unix based.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if operative system is Unix based; otherwise, <c>false</c>.
        /// </returns>
        public bool IsUnix()
        {
            var platform = (int)Environment.OSVersion.Platform;
            var isUnix = (platform == 4) || (platform == 6) || (platform == 128);
            return isUnix;
        }

        /// <summary>
        /// Gets the fully qualified path of the current working directory.
        /// </summary>
        /// <returns></returns>
        public DirectoryPath GetWorkingDirectory()
        {
            return new DirectoryPath(Environment.CurrentDirectory);
        }
    }
}