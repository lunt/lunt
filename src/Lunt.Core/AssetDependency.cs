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
using Lunt.IO;

namespace Lunt
{
    using Lunt.IO;

    /// <summary>
    /// Represent a dependency to an asset.
    /// </summary>
    public sealed class AssetDependency
    {
        private readonly FilePath _path;
        private readonly long _fileSize;
        private readonly string _checksum;

        /// <summary>
        /// Gets the path of the dependency.
        /// </summary>
        /// <value>The path of the dependency.</value>
        public FilePath Path
        {
            get { return _path; }
        }

        /// <summary>
        /// Gets the file size of the dependency.
        /// </summary>
        /// <value>The file size of the dependency.</value>
        public long FileSize
        {
            get { return _fileSize; }
        }

        /// <summary>
        /// Gets the checksum of the dependency.
        /// </summary>
        /// <value>The checksum of the dependency.</value>
        public string Checksum
        {
            get { return _checksum; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetDependency" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="fileSize">The file size.</param>
        /// <param name="checksum">The checksum.</param>
        public AssetDependency(FilePath path, long fileSize, string checksum)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (checksum == null)
            {
                throw new ArgumentNullException("checksum");
            }
            _path = path;
            _fileSize = fileSize;
            _checksum = checksum;
        }
    }
}