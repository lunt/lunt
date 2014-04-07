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
using Lunt.IO;
using LuntPath = Lunt.IO.Path;
using LuntFilePath = Lunt.IO.FilePath;

namespace Lunt
{
    using Lunt.IO;

    /// <summary>
    /// The build manifest contains information about a build.
    /// </summary>
    public sealed class BuildManifest
    {
        /// <summary>
        /// The manifest version.
        /// </summary>
        public const int ManifestVersion = 1;

        private readonly List<BuildManifestItem> _items;

        /// <summary>
        /// Gets the build manifest items.
        /// </summary>
        /// <value>The items.</value>
        public IList<BuildManifestItem> Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildManifest" /> class.
        /// </summary>
        public BuildManifest()
        {
            _items = new List<BuildManifestItem>();
        }

        /// <summary>
        /// Loads a <see cref="BuildManifest" /> from the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">stream</exception>
        public static BuildManifest Load(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            var manifest = new BuildManifest();

            using (var reader = new NoCloseBinaryReader(stream))
            {
                reader.ReadInt32(); // Version
                var itemCount = reader.ReadInt32();

                for (int i = 0; i < itemCount; i++)
                {
                    var path = reader.ReadString();
                    var status = (AssetBuildStatus) reader.ReadInt32();
                    var message = reader.ReadString();
                    var length = reader.ReadInt64();
                    var checksum = reader.ReadString();

                    // Read metadata from stream.
                    var metadataCount = reader.ReadInt32();
                    var metadata = new Dictionary<string, string>();
                    for (int j = 0; j < metadataCount; j++)
                    {
                        var key = reader.ReadString();
                        var value = reader.ReadString();
                        metadata.Add(key, value);
                    }

                    // Create the manifest item.
                    var asset = new Asset(path, metadata);
                    var item = new BuildManifestItem(asset) {
                        Message = message,
                        Status = status,
                        Length = length,
                        Checksum = checksum,
                    };

                    // Read dependencies.
                    var dependencyCount = reader.ReadInt32();
                    var dependencies = new List<AssetDependency>();
                    for (int j = 0; j < dependencyCount; j++)
                    {
                        var dependencyPath = new FilePath(reader.ReadString());
                        var dependencyFileSize = reader.ReadInt64();
                        var dependencyChecksum = reader.ReadString();

                        dependencies.Add(new AssetDependency(dependencyPath, dependencyFileSize, dependencyChecksum));
                    }
                    item.Dependencies = dependencies.ToArray();

                    manifest.Items.Add(item);
                }
            }

            return manifest;
        }

        /// <summary>
        /// Saves the <see cref="BuildManifest" /> to the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void Save(Stream stream)
        {
            using (var writer = new NoCloseBinaryWriter(stream))
            {
                writer.Write(ManifestVersion);
                writer.Write(this.Items.Count);
                foreach (var item in this.Items)
                {
                    writer.Write(item.Asset.Path.FullPath);
                    writer.Write((int) item.Status);
                    writer.WriteString(item.Message);
                    writer.Write(item.Length);
                    writer.WriteString(item.Checksum);

                    writer.Write(item.Asset.Metadata.Count);
                    foreach (var key in item.Asset.Metadata.GetKeys())
                    {
                        writer.WriteString(key);
                        writer.WriteString(item.Asset.Metadata.GetValue(key));
                    }

                    int dependencyCount = item.Dependencies != null ? item.Dependencies.Length : 0;
                    writer.Write(dependencyCount);
                    if (item.Dependencies != null)
                    {
                        foreach (var dependency in item.Dependencies)
                        {
                            writer.Write(dependency.Path.FullPath);
                            writer.Write(dependency.FileSize);
                            writer.Write(dependency.Checksum);
                        }
                    }
                }
            }
        }
    }
}