using Lake;
using Lake.Commands;
using Lake.Commands.Building;
using Lunt.Diagnostics;
using Lunt.IO;
using Lunt.Runtime;
using Moq;

namespace Lunt.Testing.Utilities
{
    internal sealed class BuildCommandFactory
    {
        public LakeOptions Options { get; set; }
        public BuildManifest Manifest { get; set; }
        public string WorkingDirectory { get; set; }
        public FakeConsole Console { get; set; }
        public Mock<IPipelineScannerFactory> ScannerFactory { get; set; }
        public Mock<IBuildEngineInvoker> BuildEngineInvoker { get; set; }
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
                ScannerFactory = new Mock<IPipelineScannerFactory>();
                ScannerFactory.Setup(x => x.Create(It.IsAny<DirectoryPath>()))
                    .Returns(new Mock<IPipelineScanner>().Object);
            }

            if (BuildEngineInvoker == null)
            {
                BuildEngineInvoker = new Mock<IBuildEngineInvoker>();
                BuildEngineInvoker.Setup(x => x.Build(It.IsAny<BuildEngine>(), It.IsAny<BuildEngineSettings>()))
                    .Returns(Manifest ?? new BuildManifest());
            }

            return new BuildCommand(log, Console, ScannerFactory.Object, BuildEnvironment, 
                BuildEngineInvoker != null ? BuildEngineInvoker.Object : null);
        }
    }
}
