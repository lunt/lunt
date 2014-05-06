using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Lunt.IO
{
    /// <summary>
    /// Computes a hash from a file or a stream of data.
    /// </summary>
    public sealed class HashComputer : IHashComputer
    {
        private readonly MD5 _hasher;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="HashComputer" /> class.
        /// </summary>
        public HashComputer()
        {
            _hasher = MD5.Create();
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="HashComputer"/> class.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _hasher.Clear();
                _disposed = true;
            }
        }

        /// <summary>
        /// Computes the hash for the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>A hash representing the specified file.</returns>
        public string Compute(IFile file)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            using (var stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var hash = _hasher.ComputeHash(stream);
                return string.Concat(hash.Select(c => c.ToString("x2")));
            }
        }
    }
}