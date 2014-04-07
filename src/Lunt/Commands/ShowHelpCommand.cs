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

namespace Lunt.Commands
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

        public int Execute()
        {
            _console.WriteLine("Usage: Lunt.exe [-input=target] [-output=target] [-probing=target]");
            _console.WriteLine("                  [-verbosity=target] [-help] [-version]");
            _console.WriteLine("                  [-rebuild] <build_configuration>");
            _console.WriteLine("");
            _console.WriteLine("Example: Lunt.exe build.config");
            _console.WriteLine("Example: Lunt.exe -verbosity=quiet -output=\"../out\" build.config");
            _console.WriteLine("         Lunt.exe -output=\"../out\" -rebuild build.config");
            _console.WriteLine("");
            _console.WriteLine("Options:");
            _console.WriteLine("    -input=target	Sets the input directory.");
            _console.WriteLine("    -output=target	Sets the output directory.");
            _console.WriteLine("    -output=target	Sets the assembly probing directory.");
            _console.WriteLine("    -verbosity=value	Specifies the amount of information to be displayed.");
            _console.WriteLine("    -help		Displays usage information.");
            _console.WriteLine("    -version		Displays detailed version information.");
            _console.WriteLine("    -rebuild		Performs a non incremental build.");

            return 0;
        }
    }
}