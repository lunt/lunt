using System.Collections.Generic;
using Lunt.Bootstrapping;
using Lunt.IO;
using Moq;

namespace Lunt.Testing
{
    public class FakeInternalConfiguration : IInternalConfiguration
    {
        public Mock<IFileSystem> FileSystem { get; set; }
        public Mock<IBuildEnvironment> Environment { get; set; }
        public Mock<IBuildConfigurationReader> BuildConfigurationReader { get; set; }
        public Mock<IBuildKernel> BuildKernel { get; set; }

        public FakeInternalConfiguration(FilePath path)
        {
            var fileSystem = new Mock<IFileSystem>();
            var file = new Mock<IFile>();
            file.Setup(x => x.Exists).Returns(true);
            fileSystem.Setup(x => x.GetFile(path)).Returns(file.Object);
            FileSystem = fileSystem;

            var environment = new Mock<IBuildEnvironment>();
            environment.Setup(x => x.GetWorkingDirectory()).Returns("/working");
            Environment = environment;

            var reader = new Mock<IBuildConfigurationReader>();
            reader.Setup(x => x.Read(It.IsAny<FilePath>())).Returns(new BuildConfiguration());
            BuildConfigurationReader = reader;

            var kernel = new Mock<IBuildKernel>();
            kernel.Setup(x => x.Build(It.IsAny<BuildConfiguration>())).Returns(new BuildManifest());
            kernel.Setup(x => x.Build(It.IsAny<BuildConfiguration>(), It.IsAny<BuildManifest>())).Returns(new BuildManifest());
            BuildKernel = kernel;
        }

        public IEnumerable<ContainerRegistration> GetRegistrations()
        {
            yield return new InstanceRegistration(typeof(IFileSystem), FileSystem.Object);
            yield return new InstanceRegistration(typeof(IBuildEnvironment), Environment.Object);
            yield return new InstanceRegistration(typeof(IBuildConfigurationReader), BuildConfigurationReader.Object);
            yield return new InstanceRegistration(typeof(IBuildKernel), BuildKernel.Object);
        }
    }
}