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
    /// Represents the build result of an asset.B
    /// </summary>
    public sealed class AssetBuildResult
    {
        private readonly BuildManifestItem _manifestItem;
        private readonly string _message;

        /// <summary>
        /// The asset that was built.
        /// </summary>
        /// <value>The asset.</value>
        public Asset Asset
        {
            get { return _manifestItem.Asset; }
        }

        /// <summary>
        /// Gets the asset build status.
        /// </summary>
        /// <value>The status.</value>
        public AssetBuildStatus Status
        {
            get { return _manifestItem.Status; }
        }

        /// <summary>
        /// Gets the built manifest item.
        /// </summary>
        /// <value>The manifest.</value>
        public BuildManifestItem ManifestItem
        {
            get { return _manifestItem; }
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message
        {
            get { return _message; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetBuildResult" /> class.
        /// </summary>
        /// <param name="manifestItem">The manifest item.</param>
        /// <param name="message">The build result message.</param>
        /// <exception cref="System.ArgumentNullException">manifestItem</exception>
        public AssetBuildResult(BuildManifestItem manifestItem, string message)
        {
            if (manifestItem == null)
            {
                throw new ArgumentNullException("manifestItem");
            }
            _manifestItem = manifestItem;
            _message = message ?? string.Empty;
        }

        /// <summary>
        /// Creates a build result representing a successfull build.
        /// </summary>
        /// <param name="manifest">The manifest.</param>
        /// <returns>A build result representing a successfull build.</returns>
        internal static AssetBuildResult Success(BuildManifestItem manifest)
        {
            manifest.Status = AssetBuildStatus.Success;
            manifest.Message = string.Empty;
            return new AssetBuildResult(manifest, string.Empty);
        }

        /// <summary>
        /// Creates a build result representing a failed build.
        /// </summary>
        /// <param name="manifest">The manifest.</param>
        /// <param name="message">The message.</param>
        /// <returns>A build result representing a failed build.</returns>
        internal static AssetBuildResult Failure(BuildManifestItem manifest, string message)
        {
            manifest.Status = AssetBuildStatus.Failure;
            manifest.Message = message;
            return new AssetBuildResult(manifest, message);
        }
    }
}