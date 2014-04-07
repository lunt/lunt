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

namespace Lunt
{
    /// <summary>
    /// The build manifest item.
    /// </summary>
    public sealed class BuildManifestItem
    {
        private readonly Asset _asset;
        private AssetBuildStatus _status;
        private string _message;
        private long _length;
        private string _checksum;
        private AssetDependency[] _dependencies;

        /// <summary>
        /// Gets the asset.
        /// </summary>
        /// <value>The asset.</value>
        public Asset Asset
        {
            get { return _asset; }
        }

        /// <summary>
        /// Gets or sets the build status.
        /// </summary>
        /// <value>The build status.</value>
        public AssetBuildStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        /// <summary>
        /// Gets or sets the build message.
        /// </summary>
        /// <value>The message.</value>
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        /// <summary>
        /// Gets or sets the file length.
        /// </summary>
        /// <value>The length.</value>
        public long Length
        {
            get { return _length; }
            set { _length = value; }
        }

        /// <summary>
        /// Gets or sets the file checksum.
        /// </summary>
        /// <value>The checksum.</value>
        public string Checksum
        {
            get { return _checksum; }
            set { _checksum = value; }
        }

        /// <summary>
        /// Gets or sets the asset dependencies.
        /// </summary>
        /// <value>
        /// The asset dependencies.
        /// </value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public AssetDependency[] Dependencies
        {
            get { return _dependencies; }
            set { _dependencies = value ?? new AssetDependency[] {}; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildManifestItem" /> class.
        /// </summary>
        /// <param name="asset">The asset.</param>
        /// <exception cref="System.ArgumentNullException">asset</exception>
        public BuildManifestItem(Asset asset)
        {
            if (asset == null)
            {
                throw new ArgumentNullException("asset");
            }
            _asset = asset;
            _status = AssetBuildStatus.Unknown;
            _message = string.Empty;
            _checksum = string.Empty;
            _length = 0;
            _dependencies = new AssetDependency[] {};
        }
    }
}