using Lunt;

namespace Lake.Tests.Integration
{
    public class IntegrationTestResult
    {
        private readonly int _exitCode;
        private readonly BuildManifest _manifest;

        public int ExitCode
        {
            get { return _exitCode; }
        }

        public BuildManifest Manifest
        {
            get { return _manifest; }
        }

        public IntegrationTestResult(int exitCode, BuildManifest manifest)
        {
            _exitCode = exitCode;
            _manifest = manifest;
        }
    }
}
