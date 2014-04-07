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
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lunt.IO
{
    /// <summary>
    /// Representation of a physical directory.
    /// </summary>
    public sealed class Directory : IDirectory
    {
        private readonly DirectoryInfo _directory;
        private readonly DirectoryPath _path;

        /// <summary>
        /// Gets the directory path.
        /// </summary>
        /// <value>The path.</value>
        public DirectoryPath Path
        {
            get { return _path; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IDirectory" /> exists.
        /// </summary>
        /// <value>
        ///   <c>true</c> if exists; otherwise, <c>false</c>.
        /// </value>
        public bool Exists
        {
            get { return _directory.Exists; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Directory" /> class.
        /// </summary>
        /// <param name="directory">The directory.</param>
        public Directory(DirectoryInfo directory)
        {
            _directory = directory;
            _path = new DirectoryPath(_directory.FullName);
        }

        /// <summary>
        /// Creates the directory.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if exists; otherwise, <c>false</c>.
        /// </returns>
        public bool Create()
        {
            try
            {
                _directory.Create();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Returns a directory list from the current directory matching the given search pattern and using a 
        /// value to determine whether to search subdirectories.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="scope">The search scope.</param>
        /// <returns></returns>
        public IEnumerable<IDirectory> GetDirectories(string filter, SearchScope scope)
        {
            SearchOption option = scope == SearchScope.Current ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories;
            return _directory.GetDirectories(filter, option).Select(directory => new Directory(directory));
        }

        /// <summary>
        /// Returns a file list from the current directory matching the given search pattern and using a 
        /// value to determine whether to search subdirectories.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="scope">The search scope.</param>
        /// <returns></returns>
        public IEnumerable<IFile> GetFiles(string filter, SearchScope scope)
        {
            SearchOption option = scope == SearchScope.Current ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories;
            return _directory.GetFiles(filter, option).Select(file => new File(file));
        }
    }
}