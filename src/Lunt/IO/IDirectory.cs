using System.Collections.Generic;

namespace Lunt.IO
{
    /// <summary>
    /// A representation of a directory.
    /// </summary>
    public interface IDirectory
    {
        /// <summary>
        /// Gets the directory path.
        /// </summary>
        /// <value>The path.</value>
        DirectoryPath Path { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IDirectory" /> exists.
        /// </summary>
        /// <value>
        ///   <c>true</c> if exists; otherwise, <c>false</c>.
        /// </value>
        bool Exists { get; }

        /// <summary>
        /// Creates the directory.
        /// </summary>
        /// <returns><c>true</c> if exists; otherwise, <c>false</c>.</returns>
        bool Create();

        /// <summary>
        /// Returns a directory list from the current directory matching the given search pattern and using a 
        /// value to determine whether to search subdirectories.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="scope">The search scope.</param>
        /// <returns></returns>
        IEnumerable<IDirectory> GetDirectories(string filter, SearchScope scope);

        /// <summary>
        /// Returns a file list from the current directory matching the given search pattern and using a 
        /// value to determine whether to search subdirectories.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="scope">The search scope.</param>
        /// <returns></returns>
        IEnumerable<IFile> GetFiles(string filter, SearchScope scope);
    }
}