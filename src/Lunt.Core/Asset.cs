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
    /// Represents a resolved asset.
    /// </summary>
    public sealed class Asset
    {
        private readonly FilePath _path;
        private readonly AssetMetadata _metadata;

        /// <summary>
        /// Gets the relative path to the asset.
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
        public AssetMetadata Metadata
        {
            get { return _metadata; }
        }

        /// <summary>
        /// Gets or sets the name of the processor to be used when building the asset.
        /// </summary>
        /// <value>
        /// The name of the processor to be used when building the asset.
        /// </value>
        public string ProcessorName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Asset" /> class.
        /// </summary>
        /// <param name="path">The relative path to the asset.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">path</exception>
        /// <exception cref="System.ArgumentNullException">metadata</exception>
        public Asset(FilePath path, IDictionary<string, string> metadata = null)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            _path = path;
            _metadata = new AssetMetadata(metadata ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets the source file path.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public FilePath GetSourceFilePath(BuildConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }
            return configuration.InputDirectory.Combine(_path);
        }

        /// <summary>
        /// Gets the target file path.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public FilePath GetTargetFilePath(BuildConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }
            return configuration.OutputDirectory.Combine(_path.ChangeExtension(".dat"));
        }
    }
}