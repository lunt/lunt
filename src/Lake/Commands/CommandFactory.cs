﻿using System;
using Lake.Runtime;
using Lunt;
using Lunt.Diagnostics;
using Lunt.IO;
using Lunt.Runtime;

namespace Lake.Commands
{
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

        public ICommand CreateHelpCommand(LakeOptions options)
        {
            return new ShowHelpCommand(_console);
        }

        public ICommand CreateVersionCommand(LakeOptions options)
        {
            return new ShowVersionCommand(_console);
        }

        public ICommand CreateBuildCommand(LakeOptions options)
        {
            return new BuildCommand(_log, _console, _factory, _environment, options);
        }
    }
}