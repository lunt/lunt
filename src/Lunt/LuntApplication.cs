// ﻿
// Copyright (c) 2013 Patrik Svensson
// 
// This file is part of Lunt.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
using System;
using System.Diagnostics;
using Lunt.Arguments;
using Lunt.Commands;
using Lunt.Diagnostics;

namespace Lunt
{
    using Lunt.Arguments;
    using Lunt.Commands;
    using Lunt.Diagnostics;

    public sealed class LuntApplication
    {
        private readonly IConsoleBuildLog _log;
        private readonly IConsoleWriter _console;
        private readonly IArgumentParser _parser;
        private readonly ICommandFactory _factory;

        public LuntApplication()
            : this(null, null, null, null)
        {
        }

        internal LuntApplication(IConsoleWriter console, IConsoleBuildLog log,
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
                _console.WriteLine("Lunt Version {0}", GetVersion());
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
                var command = this.CreateCommand(options);
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
            var assembly = typeof(LuntApplication).Assembly;
            return FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;
        }

        private ICommand CreateCommand(LuntOptions options)
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