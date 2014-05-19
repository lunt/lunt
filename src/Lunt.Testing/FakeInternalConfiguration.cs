using System;
using System.Collections.Generic;
using Lunt.Bootstrapping;
using Lunt.Diagnostics;
using Lunt.IO;
using Lunt.Runtime;
using Lunt.Tests.Framework;
using Moq;

namespace Lunt.Testing
{
    public class FakeInternalConfiguration : IInternalConfiguration
    {
        public Mock<IFileSystem> FileSystem { get; set; }
        public Mock<IBuildEnvironment> Environment { get; set; }
        public Mock<IBuildConfigurationReader> BuildConfigurationReader { get; set; }
        public Mock<IBuildKernel> BuildKernel { get; set; }
        public Mock<IBuildManifestProvider> BuildManifestProvider { get; set; }

        public FakeInternalConfiguration(FilePath path, IFile manifest = null)
        {
            var fileSystem = new Mock<IFileSystem>();
            var file = new Mock<IFile>();
            file.Setup(x => x.Exists).Returns(true);
            fileSystem.Setup(x => x.GetFile(path)).Returns(file.Object);

            if (manifest == null)
            {
                var manifestMock = new Mock<IFile>();
                manifestMock.SetupGet(x => x.Exists).Returns(false);
                manifest = manifestMock.Object;
            }
            fileSystem.Setup(x => x.GetFile(It.Is<FilePath>(f => f.FullPath.EndsWith("manifest", StringComparison.Ordinal)))).Returns(manifest);
            FileSystem = fileSystem;

            var environment = new Mock<IBuildEnvironment>();
            environment.Setup(x => x.GetWorkingDirectory()).Returns("/working");
            environment.SetupGet(x => x.FileSystem).Returns(fileSystem.Object);
            Environment = environment;

            var reader = new Mock<IBuildConfigurationReader>();
            reader.Setup(x => x.Read(It.IsAny<FilePath>())).Returns(new BuildConfiguration());
            BuildConfigurationReader = reader;

            var manifestProvider = new Mock<IBuildManifestProvider>();
            manifestProvider.Setup(x => x.LoadManifest(It.IsAny<IFileSystem>(), It.IsAny<FilePath>())).Returns((BuildManifest)null);
            BuildManifestProvider = manifestProvider;

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
            yield return new InstanceRegistration(typeof(IBuildManifestProvider), BuildManifestProvider.Object);

            yield return new TypeRegistration(typeof(IBuildLog), typeof(TraceBuildLog), Lifetime.Singleton);
            yield return new TypeRegistration(typeof(IHashComputer), typeof(HashComputer), Lifetime.Singleton);
            yield return new TypeRegistration(typeof(IPipelineScanner), typeof(FakePipelineScanner), Lifetime.Singleton);
        }
    }
}