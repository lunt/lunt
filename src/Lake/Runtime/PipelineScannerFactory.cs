using Lunt;
using Lunt.IO;
using Lunt.Diagnostics;
using Lunt.Runtime;

namespace Lake.Runtime
{
    internal sealed class PipelineScannerFactory : IPipelineScannerFactory
    {
        private readonly IFileSystem _fileSystem;
        private readonly IBuildLog _log;

        public PipelineScannerFactory(IFileSystem fileSystem, IBuildLog log)
        {
            _fileSystem = fileSystem;
            _log = log;
        }

        public IPipelineScanner Create(DirectoryPath probingPath)
        {
            var probingDirectory = _fileSystem.GetDirectory(probingPath);
            return new DirectoryScanner(_log, probingDirectory);
        }
    }
}
