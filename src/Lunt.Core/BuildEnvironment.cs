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
            int platform = (int)Environment.OSVersion.Platform;
            bool isUnix = (platform == 4) || (platform == 6) || (platform == 128);
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