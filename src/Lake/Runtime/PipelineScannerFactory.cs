using Lunt;
using Lunt.IO;
using Lunt.Diagnostics;
using Lunt.Runtime;

namespace Lake.Runtime
{
    internal sealed class PipelineScannerFactory : IPipelineScannerFactory
    {
        private readonly IBuildEnvironment _environment;
        private readonly IBuildLog _log;

        public PipelineScannerFactory(IBuildEnvironment environment, IBuildLog log)
        {
            _environment = environment;
            _log = log;
        }

        public IPipelineScanner Create(DirectoryPath probingPath)
        {
            var probingDirectory = _environment.FileSystem.GetDirectory(probingPath);
            return new PipelineDirectoryScanner(_log, probingDirectory);
        }
    }
}
