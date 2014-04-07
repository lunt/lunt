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
using Lunt.Diagnostics;
using Lunt.IO;
using Lunt.Runtime;

namespace Lunt.Commands
{
    using Lunt.Diagnostics;
    using Lunt.IO;
    using Lunt.Runtime;

    internal sealed class CommandFactory : ICommandFactory
    {
        private readonly IBuildLog _log;
        private readonly IConsoleWriter _console;
        private readonly IBuildEnvironment _environment;
        private readonly IPipelineScannerFactory _factory;

        public CommandFactory(IBuildLog log, IConsoleWriter writer)
            : this(log, writer, null, null)
        {
        }

        internal CommandFactory(IBuildLog log, IConsoleWriter console, 
            IBuildEnvironment environment, IPipelineScannerFactory factory)
        {
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }
            if (console == null)
            {
                throw new ArgumentNullException("console");
            }
            _log = log;
            _console = console;
            _environment = environment ?? new BuildEnvironment(new FileSystem());
            _factory = factory ?? new PipelineScannerFactory(_environment, _log);
        }

        public ICommand CreateHelpCommand(LuntOptions options)
        {
            return new ShowHelpCommand(_console);
        }

        public ICommand CreateVersionCommand(LuntOptions options)
        {
            return new ShowVersionCommand(_console);
        }

        public ICommand CreateBuildCommand(LuntOptions options)
        {
            return new BuildCommand(_log, _console, _factory, _environment, options);
        }
    }
}