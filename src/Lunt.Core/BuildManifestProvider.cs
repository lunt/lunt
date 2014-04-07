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
using System.IO;
using Lunt.IO;

namespace Lunt
{
    using Lunt.IO;

    /// <summary>
    /// Provides a mechanism to load and save manifest files.
    /// </summary>
    public sealed class BuildManifestProvider : IBuildManifestProvider
    {
        /// <summary>
        /// Loads a manifest file.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The manifest file path.</param>
        /// <returns>The loaded manifest.</returns>
        public BuildManifest LoadManifest(IFileSystem fileSystem, FilePath path)
        {
            var manifestFile = fileSystem.GetFile(path);
            if (manifestFile.Exists)
            {
                using (var stream = manifestFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    return BuildManifest.Load(stream);
                }
            }
            return null;
        }

        /// <summary>
        /// Saves a manifest to a file.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The file path where the manifest will be saved.</param>
        /// <param name="manifest">The manifest to be saved.</param>
        public void SaveManifest(IFileSystem fileSystem, FilePath path, BuildManifest manifest)
        {
            var file = fileSystem.GetFile(path);
            using (var stream = file.Open(FileMode.Create, FileAccess.Write, FileShare.None))
            {
                manifest.Save(stream);
            }
        }
    }
}