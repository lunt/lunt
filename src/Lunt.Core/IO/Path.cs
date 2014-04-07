﻿// ﻿
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

namespace Lunt.IO
{
    /// <summary>
    /// Represents a path.
    /// </summary>
    public abstract class Path
    {
        private readonly string _path;

        /// <summary>
        /// Gets the full path.
        /// </summary>
        /// <value>The full path.</value>
        public string FullPath
        {
            get { return _path; }
        }

        /// <summary>
        /// Gets a value indicating whether the path is relative.
        /// </summary>
        /// <value>
        /// <c>true</c> if the path is relative; otherwise, <c>false</c>.
        /// </value>
        public bool IsRelative
        {
            get { return !System.IO.Path.IsPathRooted(_path); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Path" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <exception cref="System.ArgumentNullException">path</exception>
        /// <exception cref="System.ArgumentException">Path cannot be empty.</exception>
        protected Path(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Path cannot be empty.", "path");
            }
            _path = path.Replace('\\', '/').Trim();
            _path = _path == "./" ? string.Empty : _path;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents the path.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this path.
        /// </returns>
        public override string ToString()
        {
            return this.FullPath;
        }
    }
}