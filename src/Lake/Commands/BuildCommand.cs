using System;
using System.Linq;
using Lunt;
using Lunt.Diagnostics;
using Lunt.IO;
using Lunt.Runtime;

namespace Lake.Commands
{
    internal sealed class BuildCommand : ICommand
    {
        private readonly LakeOptions _options;
        private readonly IBuildLog _log;
        private readonly IConsoleWriter _console;
        private readonly IHashComputer _hasher;
        private readonly IPipelineScannerFactory _scannerFactory;
        private readonly IBuildEnvironment _environment;
        private readonly IBuildManifestProvider _manifestProvider;
        private readonly IBuildConfigurationReader _configurationReader;
        private readonly IBuildEngine _engine;

        public BuildCommand(IBuildLog log, IConsoleWriter console,
            IPipelineScannerFactory scannerFactory,
            IBuildEnvironment environment, LakeOptions options)
        {
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }
            if (console == null)
            {
                throw new ArgumentNullException("console");
            }
            if (scannerFactory == null)
            {
                throw new ArgumentNullException("scannerFactory");
            }
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
            if (environment.FileSystem == null)
            {
                throw new ArgumentException("The build environment's file system was null.", "environment");
            }
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }
            _log = log;
            _console = console;
            _hasher = new HashComputer();
            _manifestProvider = new BuildManifestProvider();
            _environment = environment;
            _configurationReader = new BuildConfigurationXmlReader(_environment.FileSystem);
            _scannerFactory = scannerFactory;
            _options = options;
        }

        internal BuildCommand(LakeOptions options, IBuildEngine engine, IBuildLog log,
            IConsoleWriter console, IHashComputer hasher,
            IPipelineScannerFactory scannerFactory, IBuildEnvironment environment,
            IBuildManifestProvider manifestProvider, IBuildConfigurationReader configurationReader)
        {
            _options = options;
            _engine = engine;
            _log = log;
            _console = console;
            _hasher = hasher;
            _scannerFactory = scannerFactory;
            _environment = environment;
            _manifestProvider = manifestProvider;
            _configurationReader = configurationReader;
        }

        public int Execute()
        {
            if (_options.BuildConfiguration == null)
            {
                throw new LuntException("Build configuration file path has not been set.");
            }

            FixConfigurationPaths();

            // Read the build configuration.
            var configuration = _configurationReader.Read(_options.BuildConfiguration);

            // Copy settings from the console arguments to the configuration.
            configuration.InputDirectory = _options.InputDirectory;
            configuration.OutputDirectory = _options.OutputDirectory;
            configuration.Incremental = !_options.Rebuild;

            // Get the manifest path and load the previous manifest (if any).
            FilePath manifestPath = string.Concat(_options.BuildConfiguration, ".manifest");
            var previousManifest = _manifestProvider.LoadManifest(_environment.FileSystem, manifestPath);

            // Find all components and create the engine.
            var scanner = _scannerFactory.Create(GetAssemblyProbingPath());
            var components = new PipelineComponentCollection(scanner);
            var engine = _engine ?? new BuildEngine(_environment, components, _hasher, _log);

            // Perform the build.
            var manifest = engine.Build(configuration, previousManifest);

            // Save the build configuration.
            _manifestProvider.SaveManifest(_environment.FileSystem, manifestPath, manifest);

            // Output the result.
            OutputResult(manifest);

            return 0; // Success
        }

        private void FixConfigurationPaths()
        {
            // Get the working directory.
            var workingDirectory = _environment.GetWorkingDirectory();

            if (_options.BuildConfiguration.IsRelative)
            {
                // Fix the build configuration file name.
                _options.BuildConfiguration = workingDirectory.Combine(_options.BuildConfiguration);
            }

            if (_options.InputDirectory == null)
            {
                // No input directory set. Default to working directory.
                _options.InputDirectory = new DirectoryPath(workingDirectory.FullPath);
            }
            else if (_options.InputDirectory.IsRelative)
            {
                // Input directory is relative. Make it relative to the working directory.
                _options.InputDirectory = workingDirectory.Combine(_options.InputDirectory);
            }

            if (_options.OutputDirectory == null)
            {
                // No output directory set. Default to working directory.
                _options.OutputDirectory = new DirectoryPath(workingDirectory.FullPath);
            }
            else if (_options.OutputDirectory.IsRelative)
            {
                // Output directory is relative. Make it relative to the working directory.
                _options.OutputDirectory = workingDirectory.Combine(_options.OutputDirectory);
            }
        }

        private DirectoryPath GetAssemblyProbingPath()
        {
            if (_options.ProbingDirectory != null)
            {
                var probingPath = _options.ProbingDirectory;
                if (_options.ProbingDirectory.IsRelative)
                {
                    probingPath = _environment.GetWorkingDirectory().Combine(probingPath);
                }
                return probingPath;
            }
            return _environment.GetWorkingDirectory();
        }

        private void OutputResult(BuildManifest manifest)
        {
            var succeeded = manifest.Items.Count(x => x.Status == AssetBuildStatus.Success);
            var skipped = manifest.Items.Count(x => x.Status == AssetBuildStatus.Skipped);
            var failed = manifest.Items.Count - succeeded - skipped;

            _console.WriteLine("\n========== Build: {0} succeeded, {1} failed, {2} skipped ==========", succeeded, failed, skipped);
        }
    }
}