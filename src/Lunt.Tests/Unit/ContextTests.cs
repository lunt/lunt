using System;
using Lunt.Diagnostics;
using Lunt.IO;
using Lunt.Testing;
using NSubstitute;
using Xunit;

namespace Lunt.Tests.Unit
{
    public class ContextTests
    {
        public class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_File_System_Is_Null()
            {
                // Given
                var log = Substitute.For<IBuildLog>();
                var configuration = new BuildConfiguration();
                var hasher = Substitute.For<IHashComputer>();
                var asset = new Asset("simple.asset");

                // When
                var result = Record.Exception(() => new Context(null, configuration, hasher, log, asset));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("fileSystem", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Hasher_Is_Null()
            {
                // Given
                var fileSystem = Substitute.For<IFileSystem>();
                var configuration = new BuildConfiguration();
                var log = Substitute.For<IBuildLog>();
                var asset = new Asset("simple.asset");

                // When
                var result = Record.Exception(() => new Context(fileSystem, configuration, null, log, asset));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("hasher", ((ArgumentNullException)result).ParamName);
            }


            [Fact]
            public void Should_Throw_If_Build_Log_Is_Null()
            {
                // Given
                var fileSystem = Substitute.For<IFileSystem>();
                var configuration = new BuildConfiguration();
                var hasher = Substitute.For<IHashComputer>();
                var asset = new Asset("simple.asset");

                // When
                var result = Record.Exception(() => new Context(fileSystem, configuration, hasher, null, asset));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("log", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Asset_Is_Null()
            {
                // Given
                var fileSystem = Substitute.For<IFileSystem>();
                var configuration = new BuildConfiguration();
                var hasher = Substitute.For<IHashComputer>();
                var log = Substitute.For<IBuildLog>();

                // When
                var result = Record.Exception(() => new Context(fileSystem, configuration, hasher, log, null));

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
                var log = Substitute.For<IBuildLog>();
                var asset = new Asset("simple.asset");
                var context = new Context(filesystem, configuration, hasher, log, asset);

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
                var log = Substitute.For<IBuildLog>();
                var asset = new Asset("simple.asset");
                var context = new Context(filesystem, configuration, hasher, log, asset);

                var file = Substitute.For<IFile>();
                file.Exists.Returns(true);
                file.Length.Returns(12);
                file.Path.Returns("/input/other.asset");

                // When
                context.AddDependency(file);
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
                var log = Substitute.For<IBuildLog>();
                var asset = new Asset("simple.asset");
                var context = new Context(filesystem, configuration, hasher, log, asset);

                var file = Substitute.For<IFile>();
                file.Exists.Returns(true);
                file.Length.Returns(12);
                file.Path.Returns("/input/other.asset");

                // When
                context.AddDependency(file);
                context.AddDependency(file);
                var result = context.GetDependencies();

                // Then
                Assert.Equal(1, result.Length);
            }

            [Fact]
            public void Should_Throw_If_Dependency_Is_Null()
            {
                // Given
                var filesystem = Substitute.For<IFileSystem>();
                var hasher = new FakeHashComputer("ABCDEF");
                var configuration = new BuildConfiguration();
                var log = Substitute.For<IBuildLog>();
                var asset = new Asset("simple.asset");
                var context = new Context(filesystem, configuration, hasher, log, asset);

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
                var filesystem = Substitute.For<IFileSystem>();
                var hasher = new FakeHashComputer("ABCDEF");
                var configuration = new BuildConfiguration();
                var log = Substitute.For<IBuildLog>();
                var asset = new Asset("simple.asset");
                var context = new Context(filesystem, configuration, hasher, log, asset);

                var file = Substitute.For<IFile>();
                file.Exists.Returns(false);
                file.Length.Returns(12);
                file.Path.Returns("other.asset");

                // When
                var result = Record.Exception(() => context.AddDependency(file));

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
                var log = Substitute.For<IBuildLog>();
                var asset = new Asset("simple.asset");
                var context = new Context(filesystem, configuration, hasher, log, asset);

                var file = Substitute.For<IFile>();
                file.Exists.Returns(true);
                file.Length.Returns(12);
                file.Path.Returns("other.asset");

                // When
                var result = Record.Exception(() => context.AddDependency(file));

                // Then
                Assert.IsType<LuntException>(result);
                Assert.Equal("The dependency 'other.asset' is not relative to input directory.", result.Message);
            }
        }
    }
}