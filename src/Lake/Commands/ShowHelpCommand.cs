using System;
using Lunt;

namespace Lake.Commands
{
    internal sealed class ShowHelpCommand : ICommand
    {
        private readonly IConsoleWriter _console;

        public ShowHelpCommand(IConsoleWriter console)
        {
            if (console == null)
            {
                throw new ArgumentNullException("console");
            }
            _console = console;
        }

        public int Execute(LakeOptions options)
        {
            _console.WriteLine("Usage: Lake.exe [-input=target] [-output=target] [-probing=target]");
            _console.WriteLine("                [-verbosity=target] [-help] [-version] [-colors]");
            _console.WriteLine("                [-rebuild] <build_configuration>");
            _console.WriteLine("");
            _console.WriteLine("Example: Lake.exe build.config");
            _console.WriteLine("Example: Lake.exe -verbosity=quiet -output=\"../out\" build.config");
            _console.WriteLine("         Lake.exe -output=\"../out\" -rebuild build.config");
            _console.WriteLine("");
            _console.WriteLine("Options:");
            _console.WriteLine("    -input=target       Sets the input directory.");
            _console.WriteLine("    -output=target      Sets the output directory.");
            _console.WriteLine("    -probing=target     Sets the assembly probing directory.");
            _console.WriteLine("    -verbosity=value    Specifies the amount of information to be displayed.");
            _console.WriteLine("    -help               Displays usage information.");
            _console.WriteLine("    -version            Displays detailed version information.");
            _console.WriteLine("    -colors             Outputs the build log with colors.");
            _console.WriteLine("    -rebuild            Performs a non incremental build.");

            return (int)ExitCode.Success;
        }
    }
}