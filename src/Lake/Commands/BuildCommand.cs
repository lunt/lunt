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
        private readonly IBuildLog _log;
        private readonly IConsoleWriter _console;
        private readonly IHashComputer _hasher;
        private readonly IPipelineScannerFactory _scannerFactory;
        private readonly IBuildEnvironment _environment;
        private readonly IBuildManifestProvider _manifestProvider;
        private readonly IBuildConfigurationReader _configurationReader;
        private readonly IBuildKernel _kernel;

        public BuildCommand(IBuildLog log, IConsoleWriter console,
            IPipelineScannerFactory scannerFactory,
            IBuildEnvironment environment)
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

            _log = log;
            _console = console;
            _hasher = new HashComputer();
            _manifestProvider = new BuildManifestProvider();
            _environment = environment;
            _configurationReader = new BuildConfigurationReader(_environment.FileSystem);
            _scannerFactory = scannerFactory;
        }

        internal BuildCommand(IBuildKernel kernel, IBuildLog log,
            IConsoleWriter console, IHashComputer hasher,
            IPipelineScannerFactory scannerFactory, IBuildEnvironment environment,
            IBuildManifestProvider manifestProvider, IBuildConfigurationReader configurationReader)
        {
            _kernel = kernel;
            _log = log;
            _console = console;
            _hasher = hasher;
            _scannerFactory = scannerFactory;
            _environment = environment;
            _manifestProvider = manifestProvider;
            _configurationReader = configurationReader;
        }

        public int Execute(LakeOptions options)
        {
            if (options.BuildConfiguration == null)
            {
                throw new LuntException("Build configuration file path has not been set.");
            }

            FixConfigurationPaths(options);

            // Read the build configuration.
            var configuration = _configurationReader.Read(options.BuildConfiguration);

            // Copy settings from the console arguments to the configuration.
            configuration.InputDirectory = options.InputDirectory;
            configuration.OutputDirectory = options.OutputDirectory;
            configuration.Incremental = !options.Rebuild;

            // Get the manifest path and load the previous manifest (if any).
            FilePath manifestPath = string.Concat(options.BuildConfiguration, ".manifest");
            var previousManifest = _manifestProvider.LoadManifest(_environment.FileSystem, manifestPath);

            // Find all components and create the kernel.
            var scanner = _scannerFactory.Create(GetAssemblyProbingPath(options));
            var kernel = _kernel ?? new BuildKernel(_environment, scanner, _hasher, _log);

            // Perform the build.
            var manifest = kernel.Build(configuration, previousManifest);

            // Save the build configuration.
            _manifestProvider.SaveManifest(_environment.FileSystem, manifestPath, manifest);

            // Output the result.
            OutputResult(manifest);

            var hasErrors = manifest.Items.Any(x => x.Status == AssetBuildStatus.Failure);
            return (int)(hasErrors ? ExitCode.BuildFailure : ExitCode.Success);
        }

        private void FixConfigurationPaths(LakeOptions options)
        {
            // Get the working directory.
            var workingDirectory = _environment.GetWorkingDirectory();

            if (options.BuildConfiguration.IsRelative)
            {
                // Fix the build configuration file name.
                options.BuildConfiguration = workingDirectory.Combine(options.BuildConfiguration);
            }

            if (options.InputDirectory == null)
            {
                // No input directory set. Default to working directory.
                options.InputDirectory = new DirectoryPath(workingDirectory.FullPath);
            }
            else if (options.InputDirectory.IsRelative)
            {
                // Input directory is relative. Make it relative to the working directory.
                options.InputDirectory = workingDirectory.Combine(options.InputDirectory);
            }

            if (options.OutputDirectory == null)
            {
                // No output directory set. Default to working directory.
                options.OutputDirectory = new DirectoryPath(workingDirectory.FullPath);
            }
            else if (options.OutputDirectory.IsRelative)
            {
                // Output directory is relative. Make it relative to the working directory.
                options.OutputDirectory = workingDirectory.Combine(options.OutputDirectory);
            }
        }

        private DirectoryPath GetAssemblyProbingPath(LakeOptions options)
        {
            if (options.ProbingDirectory != null)
            {
                var probingPath = options.ProbingDirectory;
                if (options.ProbingDirectory.IsRelative)
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