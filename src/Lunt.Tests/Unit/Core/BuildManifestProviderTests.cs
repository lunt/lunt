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
using Moq;
using Xunit;

namespace Lunt.Tests.Unit
{
    public class BuildManifestProviderTests
    {
        public class TheTryLoadManifestMethod
        {
            [Fact]
            public void Should_Load_Previous_Existing_Manifest()
            {
                // Given
                var service = new BuildManifestProvider();
                var fileSystem = new Mock<IFileSystem>();

                var stream = new MemoryStream();
                new BuildManifest().Save(stream);
                stream.Seek(0, SeekOrigin.Begin);

                var manifestFile = new Mock<IFile>();
                manifestFile.Setup(x => x.Exists).Returns(() => true).Verifiable();
                manifestFile.Setup(x => x.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
                    .Returns(() => stream)
                    .Verifiable();

                fileSystem.Setup(x => x.GetFile(It.IsAny<FilePath>())).Returns(() => manifestFile.Object);

                // When
                var result = service.LoadManifest(fileSystem.Object, "/assets/build.config");

                // Then            
                Assert.NotNull(result);
                manifestFile.Verify();
            }

            [Fact]
            public void Should_Return_Null_If_Manifest_File_Was_Not_Found()
            {
                // Given
                var service = new BuildManifestProvider();
                var fileSystem = new Mock<IFileSystem>();

                var manifestFile = new Mock<IFile>();
                manifestFile.Setup(x => x.Exists).Returns(() => false);

                fileSystem.Setup(x => x.GetFile(It.IsAny<FilePath>())).Returns(() => manifestFile.Object);

                // When
                var result = service.LoadManifest(fileSystem.Object, "/assets/build.config");

                // Then            
                Assert.Null(result);
                manifestFile.Verify(x => x.Open(FileMode.Open, FileAccess.Read, FileShare.Read), Times.Never);
            }
        }

        public class TheSaveManifestMethod
        {
            [Fact]
            public void Should_Save_The_Manifest()
            {
                // Given
                var service = new BuildManifestProvider();
                var fileSystem = new Mock<IFileSystem>();

                var manifestFile = new Mock<IFile>();
                manifestFile.Setup(x => x.Open(FileMode.Create, FileAccess.Write, FileShare.None))
                    .Returns(() => new MemoryStream())
                    .Verifiable();

                fileSystem.Setup(x => x.GetFile(It.IsAny<FilePath>())).Returns(() => manifestFile.Object);

                // When
                service.SaveManifest(fileSystem.Object, "/assets/build.config.manifest", new BuildManifest());

                // Then            
                manifestFile.Verify();
            }
        }
    }
}