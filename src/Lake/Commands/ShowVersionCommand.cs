using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Lunt;

namespace Lake.Commands
{
    internal sealed class ShowVersionCommand : ICommand
    {
        private readonly IConsoleWriter _console;

        public ShowVersionCommand(IConsoleWriter console)
        {
            if (console == null)
            {
                throw new ArgumentNullException("console");
            }
            _console = console;
        }

        public int Execute(LakeOptions options)
        {
            OutputVersion(typeof (LakeApplication).Assembly);
            OutputVersion(typeof (IPipelineComponent).Assembly);
            return 0;
        }

        private void OutputVersion(Assembly assembly)
        {
            var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;
            _console.WriteLine(" - {0} ({1})", Path.GetFileName(assembly.Location), version);
        }
    }
}