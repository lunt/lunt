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
using System.Collections.Generic;

namespace Lunt.IO
{
    /// <summary>
    /// A representation of a directory.
    /// </summary>
    public interface IDirectory
    {
        /// <summary>
        /// Gets the directory path.
        /// </summary>
        /// <value>The path.</value>
        DirectoryPath Path { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IDirectory" /> exists.
        /// </summary>
        /// <value>
        ///   <c>true</c> if exists; otherwise, <c>false</c>.
        /// </value>
        bool Exists { get; }

        /// <summary>
        /// Creates the directory.
        /// </summary>
        /// <returns><c>true</c> if exists; otherwise, <c>false</c>.</returns>
        bool Create();

        /// <summary>
        /// Returns a directory list from the current directory matching the given search pattern and using a 
        /// value to determine whether to search subdirectories.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="scope">The search scope.</param>
        /// <returns></returns>
        IEnumerable<IDirectory> GetDirectories(string filter, SearchScope scope);

        /// <summary>
        /// Returns a file list from the current directory matching the given search pattern and using a 
        /// value to determine whether to search subdirectories.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="scope">The search scope.</param>
        /// <returns></returns>
        IEnumerable<IFile> GetFiles(string filter, SearchScope scope);
    }
}