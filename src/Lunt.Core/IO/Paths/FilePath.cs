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
using System.ComponentModel;

namespace Lunt.IO
{
    /// <summary>
    /// Represent a file path.
    /// </summary>
    [TypeConverter(typeof (FilePathTypeConverter))]
    public sealed class FilePath : Path
    {
        /// <summary>
        /// Gets a value indicating whether the path has an extension.
        /// </summary>
        /// <value>
        /// <c>true</c> if the path has an extension; otherwise, <c>false</c>.
        /// </value>
        public bool HasExtension
        {
            get { return System.IO.Path.HasExtension(this.FullPath); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilePath" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public FilePath(string path)
            : base(path)
        {
        }

        /// <summary>
        /// Gets the directory part of the path.
        /// </summary>
        /// <returns>The directory-</returns>
        public DirectoryPath GetDirectory()
        {
            var directory = System.IO.Path.GetDirectoryName(this.FullPath);
            if (string.IsNullOrWhiteSpace(directory))
            {
                directory = "./";
            }
            return new DirectoryPath(directory);
        }

        /// <summary>
        /// Gets the filename part of the path.
        /// </summary>
        /// <returns>The filename.</returns>
        public FilePath GetFilename()
        {
            var filename = System.IO.Path.GetFileName(this.FullPath);
            return new FilePath(filename);
        }

        /// <summary>
        /// Gets the extension part of the path.
        /// </summary>
        /// <returns>The extension.</returns>
        public string GetExtension()
        {
            string extension = System.IO.Path.GetExtension(this.FullPath);
            return string.IsNullOrWhiteSpace(extension) ? null : extension;
        }

        /// <summary>
        /// Changes the extension of the path.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns>The path.</returns>
        public FilePath ChangeExtension(string extension)
        {
            return new FilePath(System.IO.Path.ChangeExtension(this.FullPath, extension));
        }

        /// <summary>
        /// Implicitly converts a string to a file path.
        /// </summary>
        /// <param name="path">The path string.</param>
        /// <returns>The file path</returns>
        public static implicit operator FilePath(string path)
        {
            return FromString(path);
        }

        /// <summary>
        /// Converts a string path to a file path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static FilePath FromString(string path)
        {
            return new FilePath(path);
        }
    }
}