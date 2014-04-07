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
using IOFile = System.IO.FileInfo;
using IODirectory = System.IO.DirectoryInfo;

namespace Lunt.IO
{
    /// <summary>
    /// Representation of a physical file system.
    /// </summary>
    public sealed class FileSystem : IFileSystem
    {
        /// <summary>
        /// Gets a value indicating whether the file system is case sensitive.
        /// </summary>
        /// <value>
        /// <c>true</c> if the file system is case sensitive; otherwise, <c>false</c>.
        /// </value>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool IsCaseSensitive
        {
            get
            {
                int platform = (int) System.Environment.OSVersion.Platform;
                bool isUnix = (platform == 4) || (platform == 6) || (platform == 128);
                return isUnix;
            }
        }

        /// <summary>
        /// Gets a file representation of the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// A file representation.
        /// </returns>
        public IFile GetFile(FilePath path)
        {
            return new File(new IOFile(path.FullPath));
        }

        /// <summary>
        /// Gets a directory representation of the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// A directory representation.
        /// </returns>
        public IDirectory GetDirectory(DirectoryPath path)
        {
            return new Directory(new IODirectory(path.FullPath));
        }
    }
}