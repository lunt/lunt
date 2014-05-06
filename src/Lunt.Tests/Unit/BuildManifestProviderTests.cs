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