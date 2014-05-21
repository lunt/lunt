using System.IO;
using Lunt.IO;
using NSubstitute;
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
                var fileSystem = Substitute.For<IFileSystem>();
                var service = new BuildManifestProvider(fileSystem);

                var stream = new MemoryStream();
                new BuildManifest().Save(stream);
                stream.Seek(0, SeekOrigin.Begin);

                var manifestFile = Substitute.For<IFile>();
                manifestFile.Exists.Returns(true);
                manifestFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read).Returns(stream);

                fileSystem.GetFile(Arg.Any<FilePath>()).Returns(manifestFile);

                // When
                var result = service.LoadManifest("/assets/build.config");

                // Then            
                Assert.NotNull(result);
                manifestFile.Received(1).Open(FileMode.Open, FileAccess.Read, FileShare.Read);
            }

            [Fact]
            public void Should_Return_Null_If_Manifest_File_Was_Not_Found()
            {
                // Given
                var fileSystem = Substitute.For<IFileSystem>();
                var service = new BuildManifestProvider(fileSystem);

                var manifestFile = Substitute.For<IFile>();
                manifestFile.Exists.Returns(false);

                fileSystem.GetFile(Arg.Any<FilePath>()).Returns(manifestFile);

                // When
                var result = service.LoadManifest("/assets/build.config");

                // Then            
                Assert.Null(result);
                manifestFile.DidNotReceive().Open(FileMode.Open, FileAccess.Read, FileShare.Read);
            }
        }

        public class TheSaveManifestMethod
        {
            [Fact]
            public void Should_Save_The_Manifest()
            {
                // Given
                var fileSystem = Substitute.For<IFileSystem>();
                var service = new BuildManifestProvider(fileSystem);

                var manifestFile = Substitute.For<IFile>();
                manifestFile.Open(FileMode.Create, FileAccess.Write, FileShare.None)
                    .Returns(r => new MemoryStream());

                fileSystem.GetFile(Arg.Any<FilePath>()).Returns(manifestFile);

                // When
                service.SaveManifest("/assets/build.config.manifest", new BuildManifest());

                // Then            
                manifestFile.Received(1).Open(FileMode.Create, FileAccess.Write, FileShare.None);
            }
        }
    }
}