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
using Lunt.Diagnostics;
using Lunt.IO;
using Lunt.Tests.Fakes;
using Moq;
using Xunit;

namespace Lunt.Tests.Unit
{
    public class LuntContextTests
    {
        public class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_File_System_Is_Null()
            {
                // Given
                var log = new Mock<IBuildLog>().Object;
                var configuration = new BuildConfiguration();
                var hasher = new Mock<IHashComputer>().Object;
                var asset = new Asset("simple.asset");

                // When
                var result = Record.Exception(() => new LuntContext(null, configuration, hasher, log, asset));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("fileSystem", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Hasher_Is_Null()
            {
                // Given
                var fileSystem = new Mock<IFileSystem>().Object;
                var configuration = new BuildConfiguration();
                var log = new Mock<IBuildLog>().Object;
                var asset = new Asset("simple.asset");

                // When
                var result = Record.Exception(() => new LuntContext(fileSystem, configuration, null, log, asset));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("hasher", ((ArgumentNullException)result).ParamName);
            }


            [Fact]
            public void Should_Throw_If_Build_Log_Is_Null()
            {
                // Given
                var fileSystem = new Mock<IFileSystem>().Object;
                var configuration = new BuildConfiguration();
                var hasher = new Mock<IHashComputer>().Object;
                var asset = new Asset("simple.asset");

                // When
                var result = Record.Exception(() => new LuntContext(fileSystem, configuration, hasher, null, asset));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("log", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Asset_Is_Null()
            {
                // Given
                var fileSystem = new Mock<IFileSystem>().Object;
                var configuration = new BuildConfiguration();
                var hasher = new Mock<IHashComputer>().Object;
                var log = new Mock<IBuildLog>().Object;

                // When
                var result = Record.Exception(() => new LuntContext(fileSystem, configuration, hasher, log, null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("asset", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Be_Able_To_Get_Input_Directory_Path_From_Configuration()
            {
                // Given
                var filesystem = new FakeFileSystem();
                var hasher = new FakeHashComputer("ABCDEF");
                var configuration = new BuildConfiguration();
                configuration.InputDirectory = "/input";
                var log = new Mock<IBuildLog>().Object;
                var asset = new Asset("simple.asset");
                var context = new LuntContext(filesystem, configuration, hasher, log, asset);

                // When
                var result = context.InputDirectory;

                // Then
                Assert.Equal("/input", result.FullPath);
            }

            [Fact]
            public void Should_Be_Able_To_Retrive_Dependencies_Added_During_Build()
            {
                // Given
                var filesystem = new FakeFileSystem();
                var hasher = new FakeHashComputer("ABCDEF");
                var configuration = new BuildConfiguration();
                configuration.InputDirectory = "/input";
                var log = new Mock<IBuildLog>().Object;
                var asset = new Asset("simple.asset");
                var context = new LuntContext(filesystem, configuration, hasher, log, asset);

                var file = new Mock<IFile>();
                file.SetupGet(x => x.Exists).Returns(true);
                file.SetupGet(x => x.Length).Returns(12);
                file.SetupGet(x => x.Path).Returns("/input/other.asset");

                // When
                context.AddDependency(file.Object);
                var result = context.GetDependencies();

                // Then
                Assert.Equal(1, result.Length);
                Assert.Equal("ABCDEF", result[0].Checksum);
                Assert.Equal(12, result[0].FileSize);
                Assert.Equal("other.asset", result[0].Path.FullPath);
            }

            [Fact]
            public void Should_Ignore_Dependency_If_Already_Added()
            {
                // Given
                var filesystem = new FakeFileSystem();
                var hasher = new FakeHashComputer("ABCDEF");
                var configuration = new BuildConfiguration();
                configuration.InputDirectory = "/input/";
                var log = new Mock<IBuildLog>().Object;
                var asset = new Asset("simple.asset");
                var context = new LuntContext(filesystem, configuration, hasher, log, asset);

                var file = new Mock<IFile>();
                file.SetupGet(x => x.Exists).Returns(true);
                file.SetupGet(x => x.Length).Returns(12);
                file.SetupGet(x => x.Path).Returns("/input/other.asset");

                // When
                context.AddDependency(file.Object);
                context.AddDependency(file.Object);
                var result = context.GetDependencies();

                // Then
                Assert.Equal(1, result.Length);
            }

            [Fact]
            public void Should_Throw_If_Dependency_Is_Null()
            {
                // Given
                var filesystem = new Mock<IFileSystem>().Object;
                var hasher = new FakeHashComputer("ABCDEF");
                var configuration = new BuildConfiguration();
                var log = new Mock<IBuildLog>().Object;
                var asset = new Asset("simple.asset");
                var context = new LuntContext(filesystem, configuration, hasher, log, asset);

                // When
                var result = Record.Exception(() => context.AddDependency(null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("file", ((ArgumentNullException)result).ParamName);
            }


            [Fact]
            public void Should_Throw_If_Adding_Dependency_That_Does_Not_Exist()
            {
                // Given
                var filesystem = new Mock<IFileSystem>().Object;
                var hasher = new FakeHashComputer("ABCDEF");
                var configuration = new BuildConfiguration();
                var log = new Mock<IBuildLog>().Object;
                var asset = new Asset("simple.asset");
                var context = new LuntContext(filesystem, configuration, hasher, log, asset);

                var file = new Mock<IFile>();
                file.SetupGet(x => x.Path).Returns(new FilePath("other.asset"));
                file.SetupGet(x => x.Exists).Returns(false);

                // When
                var result = Record.Exception(() => context.AddDependency(file.Object));

                // Then
                Assert.IsType<LuntException>(result);
                Assert.Equal("The dependency 'other.asset' does not exist.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Dependency_Is_Not_Relative_To_The_Input_Directory()
            {
                // Given
                var filesystem = new FakeFileSystem();
                var hasher = new FakeHashComputer("ABCDEF");
                var configuration = new BuildConfiguration();
                configuration.InputDirectory = "/input/";
                var log = new Mock<IBuildLog>().Object;
                var asset = new Asset("simple.asset");
                var context = new LuntContext(filesystem, configuration, hasher, log, asset);

                var file = new Mock<IFile>();
                file.SetupGet(x => x.Exists).Returns(true);
                file.SetupGet(x => x.Length).Returns(12);
                file.SetupGet(x => x.Path).Returns("other.asset");

                // When
                var result = Record.Exception(() => context.AddDependency(file.Object));

                // Then
                Assert.IsType<LuntException>(result);
                Assert.Equal("The dependency 'other.asset' is not relative to input directory.", result.Message);
            }
        }
    }
}