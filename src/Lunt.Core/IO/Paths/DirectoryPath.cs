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
using System.ComponentModel;

namespace Lunt.IO
{
    /// <summary>
    /// Represents a directory path.
    /// </summary>
    [TypeConverter(typeof(DirectoryPathTypeConverter))]
    public sealed class DirectoryPath : Path
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryPath" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public DirectoryPath(string path)
            : base(path)
        {
        }

        /// <summary>
        /// Combines the current directory path with a file path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The combined file path.</returns>
        public FilePath Combine(FilePath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (!path.IsRelative)
            {
                throw new InvalidOperationException("Cannot combine a directory path with an absolute file path.");
            }
            var combinedPath = System.IO.Path.Combine(this.FullPath, path.FullPath);
            return new FilePath(combinedPath);
        }

        /// <summary>
        /// Combines the current directory path with another (relative) directory path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The combined directory path.</returns>
        public DirectoryPath Combine(DirectoryPath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (!path.IsRelative)
            {
                throw new InvalidOperationException("Cannot combine a directory path with an absolute directory path.");
            }
            var combinedPath = System.IO.Path.Combine(this.FullPath, path.FullPath);
            return new DirectoryPath(combinedPath);
        }

        /// <summary>
        /// Implicitly converts a string path to a directory path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A directory path.</returns>
        public static implicit operator DirectoryPath(string path)
        {
            return FromString(path);
        }

        /// <summary>
        /// Converts a string path to a directory path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static DirectoryPath FromString(string path)
        {
            return new DirectoryPath(path);
        }
    }
}