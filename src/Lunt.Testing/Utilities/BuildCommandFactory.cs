using Lake;
using Lake.Commands;
using Lake.Commands.Building;
using Lunt.Diagnostics;
using Lunt.IO;
using Lunt.Runtime;
using Moq;
using NSubstitute;

namespace Lunt.Testing.Utilities
{
    internal sealed class BuildCommandFactory
    {
        public LakeOptions Options { get; set; }
        public BuildManifest Manifest { get; set; }
        public string WorkingDirectory { get; set; }
        public FakeConsole Console { get; set; }
        public IPipelineScannerFactory ScannerFactory { get; set; }
        public IBuildEngineInvoker BuildEngineInvoker { get; set; }
        public FakeBuildEnvironment BuildEnvironment { get; set; }

        public BuildCommandFactory()
        {
            Manifest = new BuildManifest();
            Options = new LakeOptions();
            Options.BuildConfiguration = "/build.config";
            Options.InputDirectory = "/input";
            Options.OutputDirectory = "/output";
            WorkingDirectory = "/working";
            BuildEnvironment = new FakeBuildEnvironment();
            Console = new FakeConsole();
        }

        public BuildCommand CreateCommand()
        {
            var log = new Mock<IBuildLog>().Object;

            if (ScannerFactory == null)
            {
                ScannerFactory = Substitute.For<IPipelineScannerFactory>();
                ScannerFactory.Create(Arg.Any<DirectoryPath>()).Returns(Substitute.For<IPipelineScanner>());
            }

            if (BuildEngineInvoker == null)
            {
                BuildEngineInvoker = Substitute.For<IBuildEngineInvoker>();
                BuildEngineInvoker.Build(Arg.Any<BuildEngine>(), Arg.Any<BuildEngineSettings>())
                    .Returns(Manifest ?? new BuildManifest());
            }

            return new BuildCommand(log, Console, ScannerFactory, BuildEnvironment, BuildEngineInvoker ?? null);
        }
    }
}
