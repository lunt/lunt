using Lunt.IO;

namespace Lunt.Runtime
{
    public interface IPipelineScannerFactory
    {
        IPipelineScanner Create(DirectoryPath probingPath);
    }
}
