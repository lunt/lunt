using System;

namespace Lunt.IO
{
    /// <summary>
    /// Computes a hash from a file or a stream of data.
    /// </summary>
    public interface IHashComputer : IDisposable
    {
        /// <summary>
        /// Computes the hash for the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>A hash representing the specified file.</returns>
        string Compute(IFile file);
    }
}