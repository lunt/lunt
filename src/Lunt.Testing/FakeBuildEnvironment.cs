using Lunt.IO;

namespace Lunt.Testing
{
    public class FakeBuildEnvironment : IBuildEnvironment
    {
        private readonly FakeFileSystem _fileSystem;
        private readonly string _workingDirectory;
        private readonly bool _isUnix;

        public FakeBuildEnvironment(FakeFileSystem fileSystem = null, string workingDirectory = null, bool isUnix = false)
        {
            _fileSystem = fileSystem ?? new FakeFileSystem();
            _workingDirectory = workingDirectory ?? "/Working";
            _isUnix = isUnix;
        }

        public IFileSystem FileSystem
        {
            get { return _fileSystem; }
        }

        public bool IsUnix()
        {
            return _isUnix;
        }

        public DirectoryPath GetWorkingDirectory()
        {
            return _workingDirectory;
        }
    }
}
