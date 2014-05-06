using Lake;
using Lake.Commands;
using Lunt.Diagnostics;
using Lunt.IO;
using Lunt.Runtime;
using Moq;

namespace Lunt.Testing
{
    internal sealed class BuildCommandFactory
    {
        private readonly LakeOptions _options;
        private readonly BuildManifest _manifest;
        private readonly string _workingDirectory;
        private readonly BuildConfiguration _configuration;
        private readonly Mock<IBuildConfigurationReader> _configurationReader;
        private readonly Mock<IBuildEngine> _engine;
        private readonly Mock<IBuildEnvironment> _environment;
        private readonly Mock<IBuildManifestProvider> _manifestProvider;
        private readonly Mock<IPipelineScannerFactory> _scannerFactory;
        private readonly FakeConsole _console;

        public LakeOptions Options
        {
            get { return _options; }
        }

        public BuildManifest Manifest
        {
            get { return _manifest; }
        }

        public string WorkingDirectory
        {
            get { return _workingDirectory; }
        }

        public BuildConfiguration Configuration
        {
            get { return _configuration; }
        }

        public Mock<IBuildConfigurationReader> ConfigurationReader
        {
            get { return _configurationReader; }
        }

        public Mock<IBuildEngine> Engine
        {
            get { return _engine; }
        }

        public FakeConsole Console
        {
            get { return _console; }
        }

        public Mock<IPipelineScannerFactory> ScannerFactory
        {
            get { return _scannerFactory; }
        }

        public BuildCommandFactory()
        {
            _manifest = new BuildManifest();
            _options = new LakeOptions();
            _options.BuildConfiguration = "/build.config";
            _options.InputDirectory = "/input";
            _options.OutputDirectory = "/output";
            _workingDirectory = "/working";
            _configuration = new BuildConfiguration();
            _configurationReader = new Mock<IBuildConfigurationReader>();
            _engine = new Mock<IBuildEngine>();
            _environment = new Mock<IBuildEnvironment>();
            _manifestProvider = new Mock<IBuildManifestProvider>();
            _scannerFactory = new Mock<IPipelineScannerFactory>();
            _console = new FakeConsole();
        }

        public BuildCommand Create(IBuildConfigurationReader reader = null, 
            IBuildManifestProvider manifestProvider = null, 
            IPipelineScannerFactory factory = null, IBuildEngine engine = null)
        {
            var log = new Mock<IBuildLog>().Object;
            var hasher = new Mock<IHashComputer>().Object;

            if (factory == null)
            {
                ScannerFactory.Setup(x => x.Create(It.IsAny<DirectoryPath>()))
                    .Returns(new Mock<IPipelineScanner>().Object);
            }

            if (reader == null)
            {
                _configurationReader.Setup(x => x.Read(It.IsAny<FilePath>()))
                    .Returns(_configuration);
            }

            if (engine == null)
            {
                _engine.Setup(x => x.Build(It.IsAny<BuildConfiguration>(), It.IsAny<BuildManifest>()))
                    .Returns(_manifest);
            }

            _environment.Setup(x => x.GetWorkingDirectory())
                .Returns(() => _workingDirectory)
                .Verifiable();

            return new BuildCommand(_options, engine ?? _engine.Object, 
                log, _console, hasher, 
                factory ?? ScannerFactory.Object,
                _environment.Object, 
                manifestProvider ?? _manifestProvider.Object, 
                reader ?? _configurationReader.Object);
        }
    }
}
