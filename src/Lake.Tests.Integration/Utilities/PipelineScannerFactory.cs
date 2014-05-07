using Lunt.Diagnostics;
using Lunt.IO;
using Lunt.Runtime;

namespace Lake.Tests.Integration
{
    public class PipelineScannerFactory : IPipelineScannerFactory
    {
        private readonly IBuildLog _log;

        public PipelineScannerFactory(IBuildLog log)
        {
            _log = log;
        }

        public IPipelineScanner Create(DirectoryPath probingPath)
        {
            return new AssemblyScanner(_log, typeof(PipelineScannerFactory).Assembly);
        }
    }
}