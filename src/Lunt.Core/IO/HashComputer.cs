// ﻿
// Copyright (c) 2013 Patrik Svensson
// 
// This file is part of Lunt.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
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
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            using (var stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var hash = _hasher.ComputeHash(stream);
                return string.Concat(hash.Select(c => c.ToString("x2")));
            }
        }
    }
}