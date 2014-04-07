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
using Lunt.IO;

namespace Lunt
{
    using Lunt.IO;

    /// <summary>
    /// Represents a build configuration.
    /// </summary>
    public sealed class BuildConfiguration
    {
        private readonly List<AssetDefinition> _assets;

        /// <summary>
        /// Gets or sets the input directory.
        /// </summary>
        /// <value>The input directory.</value>
        public DirectoryPath InputDirectory { get; set; }

        /// <summary>
        /// Gets or sets the output directory.
        /// </summary>
        /// <value>The output directory.</value>
        public DirectoryPath OutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a build is incremental.
        /// </summary>
        /// <value>
        ///   <c>true</c> if incremental; otherwise, <c>false</c>.
        /// </value>
        public bool Incremental { get; set; }

        /// <summary>
        /// Gets the assets collection.
        /// </summary>
        /// <value>The assets collection.</value>
        public IList<AssetDefinition> Assets
        {
            get { return _assets; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildConfiguration" /> class.
        /// </summary>
        public BuildConfiguration()
        {
            _assets = new List<AssetDefinition>();
        }
    }
}