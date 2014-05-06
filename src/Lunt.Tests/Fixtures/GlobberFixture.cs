using Lunt.Testing;

namespace Lunt.Tests.Fixtures
{
    public class GlobberFixture
    {
        private readonly FakeFileSystem _fileSystem;

        public FakeFileSystem FileSystem
        {
            get { return _fileSystem; }
        }

        public GlobberFixture()
        {
            _fileSystem = CreateFileSystem();
        }

        private static FakeFileSystem CreateFileSystem()
        {
            var fileSystem = new FakeFileSystem();
            fileSystem.GetCreatedDirectory("/Temp");
            fileSystem.GetCreatedDirectory("/Temp/Hello");
            fileSystem.GetCreatedDirectory("/Temp/Hello/World");
            fileSystem.GetCreatedDirectory("/Temp/Goodbye");
            fileSystem.GetCreatedFile("/Presentation.ppt");
            fileSystem.GetCreatedFile("/Budget.xlsx");
            fileSystem.GetCreatedFile("/Text.txt");
            fileSystem.GetCreatedFile("/Temp");
            fileSystem.GetCreatedFile("/Temp/Hello/World/Text.txt");
            fileSystem.GetCreatedFile("/Temp/Hello/World/Picture.png");
            fileSystem.GetCreatedFile("/Temp/Goodbye/OtherText.txt");
            fileSystem.GetCreatedFile("/Temp/Goodbye/OtherPicture.png");
            return fileSystem;
        }
    }
}
