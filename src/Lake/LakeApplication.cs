using System;
using System.Diagnostics;
using Lake.Arguments;
using Lake.Commands;
using Lake.Diagnostics;
using Lunt;
using Lunt.Diagnostics;

namespace Lake
{
    public sealed class LakeApplication
    {
        private readonly IConsoleBuildLog _log;
        private readonly IConsoleWriter _console;
        private readonly IArgumentParser _parser;
        private readonly ICommandFactory _factory;

        public LakeApplication()
            : this(null, null, null, null)
        {
        }

        internal LakeApplication(IConsoleWriter console, IConsoleBuildLog log,
            IArgumentParser parser, ICommandFactory factory)
        {
            _console = console ?? new ConsoleWriter();
            _log = log ?? new ConsoleBuildLog(_console);
            _parser = parser ?? new ArgumentParser(_log);
            _factory = factory ?? new CommandFactory(_log, _console);
        }

        public int Run(string[] args)
        {
            try
            {
                _console.SetForeground(ConsoleColor.White);
                _console.WriteLine("Lake Version {0}", GetVersion());
                _console.ResetColors();
                _console.WriteLine("");

                // Parse options.
                var options = _parser.Parse(args);
                if (options != null)
                {
                    // Update the log with the parsed options.
                    _log.Verbosity = options.Verbosity;
                }

                // Create and execute the command.
                var command = CreateCommand(options);
                return command.Execute();
            }
            catch (Exception ex)
            {
                _log.Error("An unhandled exception occured.");
                _log.Error(ex.Message);
                return 1;
            }
        }

        private static string GetVersion()
        {
            var assembly = typeof(LakeApplication).Assembly;
            return FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;
        }

        private ICommand CreateCommand(LakeOptions options)
        {
            if (options != null)
            {
                // Show help?
                if (options.ShowHelp)
                {
                    return _factory.CreateHelpCommand(options);
                }

                // Got a build configuration?
                if (options.BuildConfiguration != null)
                {
                    return _factory.CreateBuildCommand(options);
                }

                // Show version?
                if (options.ShowVersion)
                {
                    return _factory.CreateVersionCommand(options);
                }
            }

            // Show usage as a last resort.
            return _factory.CreateHelpCommand(options);
        }
    }
}