using System;
using System.Collections.Generic;
using Lunt.Bootstrapping;
using Lunt.Diagnostics;
using Lunt.IO;
using Lunt.Runtime;
using Lunt.Tests.Framework;
using NSubstitute;

namespace Lunt.Testing
{
    public class FakeInternalConfiguration : IInternalConfiguration
    {
        public IFileSystem FileSystem { get; set; }
        public IBuildEnvironment Environment { get; set; }
        public IBuildConfigurationReader BuildConfigurationReader { get; set; }
        public IBuildKernel BuildKernel { get; set; }
        public IBuildManifestProvider BuildManifestProvider { get; set; }

        public FakeInternalConfiguration(FilePath path, IFile manifest = null)
        {
            FileSystem = CreateFileSystem(path, manifest);
            Environment = CreateBuildEnvironment(FileSystem);
            BuildConfigurationReader = CreateBuildConfigurationReader();
            BuildManifestProvider = CreateManifestProvider();
            BuildKernel = CreateBuildKernel();
        }

        public IEnumerable<ContainerRegistration> GetRegistrations()
        {
            yield return new InstanceRegistration(typeof(IFileSystem), FileSystem);
            yield return new InstanceRegistration(typeof(IBuildEnvironment), Environment);
            yield return new InstanceRegistration(typeof(IBuildConfigurationReader), BuildConfigurationReader);
            yield return new InstanceRegistration(typeof(IBuildKernel), BuildKernel);
            yield return new InstanceRegistration(typeof(IBuildManifestProvider), BuildManifestProvider);

            yield return new TypeRegistration(typeof(IBuildLog), typeof(TraceBuildLog), Lifetime.Singleton);
            yield return new TypeRegistration(typeof(IHashComputer), typeof(HashComputer), Lifetime.Singleton);
            yield return new TypeRegistration(typeof(IPipelineScanner), typeof(FakePipelineScanner), Lifetime.Singleton);
        }

        private static IBuildKernel CreateBuildKernel()
        {
            var kernel = Substitute.For<IBuildKernel>();
            kernel.Build(Arg.Any<BuildConfiguration>()).Returns(new BuildManifest());
            kernel.Build(Arg.Any<BuildConfiguration>(), Arg.Any<BuildManifest>()).Returns(new BuildManifest());
            return kernel;
        }

        private static IBuildManifestProvider CreateManifestProvider()
        {
            var manifestProvider = Substitute.For<IBuildManifestProvider>();
            manifestProvider.LoadManifest(Arg.Any<FilePath>()).Returns((BuildManifest)null);
            return manifestProvider;
        }

        private static IBuildConfigurationReader CreateBuildConfigurationReader()
        {
            var reader = Substitute.For<IBuildConfigurationReader>();
            reader.Read(Arg.Any<FilePath>()).Returns(new BuildConfiguration());
            return reader;
        }

        private static IBuildEnvironment CreateBuildEnvironment(IFileSystem fileSystem)
        {
            var environment = Substitute.For<IBuildEnvironment>();
            environment.GetWorkingDirectory().Returns("/working");
            environment.FileSystem.Returns(fileSystem);
            return environment;
        }

        private static IFileSystem CreateFileSystem(FilePath path, IFile manifest)
        {
            var fileSystem = Substitute.For<IFileSystem>();

            var file = Substitute.For<IFile>();
            file.Exists.Returns(true);
            fileSystem.GetFile(path).Returns(file);

            if (manifest == null)
            {
                manifest = Substitute.For<IFile>();
                manifest.Exists.Returns(false);
            }

            fileSystem.GetFile(Arg.Is<FilePath>(f => f.FullPath.EndsWith("manifest", StringComparison.Ordinal)))
                .Returns(manifest);

            return fileSystem;
        }
    }
}