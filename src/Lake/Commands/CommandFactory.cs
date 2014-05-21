using System;
using Lunt;
using Lunt.Diagnostics;
using Lunt.Runtime;

namespace Lake.Commands
{
    internal sealed class CommandFactory : ICommandFactory
    {
        private readonly IBuildLog _log;
        private readonly IConsoleWriter _console;
        private readonly IBuildEnvironment _environment;
        private readonly IPipelineScannerFactory _factory;

        public CommandFactory(IBuildLog log, IConsoleWriter console, 
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
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }
            _log = log;
            _console = console;
            _environment = environment;
            _factory = factory;
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
            return new BuildCommand(_log, _console, _factory, _environment);
        }
    }
}