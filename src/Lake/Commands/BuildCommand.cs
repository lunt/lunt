using System;
using System.Linq;
using Lake.Commands.Building;
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
        private readonly IPipelineScannerFactory _scannerFactory;
        private readonly IBuildEnvironment _environment;
        private readonly IBuildEngineInvoker _invoker;

        public BuildCommand(IBuildLog log, IConsoleWriter console,
            IPipelineScannerFactory scannerFactory,
            IBuildEnvironment environment, IBuildEngineInvoker invoker = null)
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
            _scannerFactory = scannerFactory;
            _environment = environment;

            // This is a hack to make the build command more testable.
            // Since we don't want to do an end-to-end testing here, we
            // leave the responsibility for invoking the build engine to
            // someone else. This way we can mock away the invokation itself.
            // Should probably be rewritten...
            _invoker = invoker ?? new BuildEngineInvoker();
        }

        public int Execute(LakeOptions options)
        {
            if (options.BuildConfiguration == null)
            {
                throw new LuntException("Build configuration file path has not been set.");
            }

            // Create the build engine settings.
            var settings = new BuildEngineSettings(options.BuildConfiguration);
            settings.Incremental = !options.Rebuild;
            settings.InputPath = options.InputDirectory;
            settings.OutputPath = options.OutputDirectory;

            // Create the internal configuration.
            var scanner = _scannerFactory.Create(GetAssemblyProbingPath(options));
            var config = new LakeInternalConfiguration(_log, _environment, scanner);

            // Create the build engine.
            var engine = new BuildEngine(config);
            var manifest = _invoker.Build(engine, settings);

            // Output the result.
            OutputResult(manifest);

            // Return result code.
            var hasErrors = manifest.Items.Any(x => x.Status == AssetBuildStatus.Failure);
            return (int)(hasErrors ? ExitCode.BuildFailure : ExitCode.Success);
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