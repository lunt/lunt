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
using Xunit;

namespace Lunt.Tests.Unit
{
    public class BuildManifestTests
    {
        [Fact]
        public void Can_Save_Manifest_To_Stream()
        {
            // Given
            var metadata = new Dictionary<string, string> {{"key", "value"}};
            var item = new BuildManifestItem(new Asset("shaders/basic.vert", metadata));
            item.Status = AssetBuildStatus.Success;
            item.Message = "Success";
            item.Length = 256;
            item.Checksum = "ABC";
            item.Dependencies = new[] {new AssetDependency("other.asset", 123, "ABC")};
            var manifest = new BuildManifest();
            manifest.Items.Add(item);

            // When
            using (var stream = new MemoryStream())
            {
                manifest.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);
                var reader = new BinaryReader(stream);

                // Then
                Assert.Equal(BuildManifest.ManifestVersion, reader.ReadInt32());
                Assert.Equal(1, reader.ReadInt32());
                Assert.Equal("shaders/basic.vert", reader.ReadString());
                Assert.Equal(AssetBuildStatus.Success, (AssetBuildStatus) reader.ReadInt32());
                Assert.Equal("Success", reader.ReadString());
                Assert.Equal(256, reader.ReadInt64());
                Assert.Equal("ABC", reader.ReadString());
                Assert.Equal(1, reader.ReadInt32());
                Assert.Equal("key", reader.ReadString());
                Assert.Equal("value", reader.ReadString());
                Assert.Equal(1, reader.ReadInt32());
                Assert.Equal("other.asset", reader.ReadString());
                Assert.Equal(123, reader.ReadInt64());
                Assert.Equal("ABC", reader.ReadString());
            }
        }

        [Fact]
        public void Should_Throw_If_Provided_Stream_Is_Null()
        {
            // Given, When
            var result = Record.Exception(() => BuildManifest.Load(null));

            // Then
            Assert.IsType<ArgumentNullException>(result);
            Assert.Equal("stream", ((ArgumentNullException) result).ParamName);
        }

        [Fact]
        public void Can_Load_Manifest_From_Stream()
        {
            // Given
            var metadata = new Dictionary<string, string> {{"key", "value"}};
            var asset = new Asset("shaders/basic.vert", metadata);
            var item = new BuildManifestItem(asset);
            item.Status = AssetBuildStatus.Success;
            item.Length = 256;
            item.Checksum = "ABC";
            item.Message = "Success";
            item.Dependencies = new[] {new AssetDependency("other.asset", 123, "ABC")};
            var manifest = new BuildManifest();
            manifest.Items.Add(item);

            // When
            using (var stream = new MemoryStream())
            {
                manifest.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);
                var loadedManifest = BuildManifest.Load(stream);

                // Then
                Assert.Equal(1, loadedManifest.Items.Count);
                Assert.Equal("shaders/basic.vert", loadedManifest.Items[0].Asset.Path.ToString());
                Assert.Equal(AssetBuildStatus.Success, loadedManifest.Items[0].Status);
                Assert.Equal("Success", loadedManifest.Items[0].Message);
                Assert.Equal(256, loadedManifest.Items[0].Length);
                Assert.Equal("ABC", loadedManifest.Items[0].Checksum);

                Assert.Equal(1, loadedManifest.Items[0].Asset.Metadata.Count);
                Assert.Equal("value", loadedManifest.Items[0].Asset.Metadata.GetValue("key"));

                Assert.NotNull(loadedManifest.Items[0].Dependencies);
                Assert.Equal(1, loadedManifest.Items[0].Dependencies.Length);
                Assert.Equal("other.asset", loadedManifest.Items[0].Dependencies[0].Path.FullPath);
                Assert.Equal(123, loadedManifest.Items[0].Dependencies[0].FileSize);
                Assert.Equal("ABC", loadedManifest.Items[0].Dependencies[0].Checksum);
            }
        }
    }
}