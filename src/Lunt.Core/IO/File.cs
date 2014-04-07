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