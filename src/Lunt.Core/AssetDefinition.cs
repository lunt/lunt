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
using Lunt.IO;

namespace Lunt
{
    using Lunt.IO;

    /// <summary>
    /// Represents a yet to be resolved asset.
    /// </summary>
    public sealed class AssetDefinition 
    {
        private readonly FilePath _path;
        private readonly bool _isGlob;
        private readonly IDictionary<string, string> _metadata;
        private string _processorName;

        /// <summary>
        /// Gets the path to the asset.
        /// </summary>
        /// <value>The path.</value>
        public FilePath Path
        {
            get { return _path; }
        }

        /// <summary>
        /// Gets the asset metadata.
        /// </summary>
        /// <value>The metadata.</value>
        public IDictionary<string, string> Metadata
        {
            get { return _metadata; }
        }

        /// <summary>
        /// Gets or sets the name of the processor to be used when building the asset.
        /// </summary>
        /// <value>
        /// The name of the processor to be used when building the asset.
        /// </value>
        public string ProcessorName
        {
            get { return _processorName; }
            set { _processorName = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the path is a glob pattern.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the path is a glob pattern; otherwise, <c>false</c>.
        /// </value>
        public bool IsGlob
        {
            get { return _isGlob; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetDefinition"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="metadata">The metadata.</param>
        public AssetDefinition(FilePath path, IDictionary<string, string> metadata = null)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            _path = path;
            _metadata = metadata ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            _isGlob = path.FullPath.Contains("*") || path.FullPath.Contains("?");
        }
    }
}
