using System.Collections.Generic;
using System.IO;
using Lunt.IO;

namespace Lunt.Testing
{
    public class FakeFileSystem : IFileSystem
    {
        private readonly Dictionary<DirectoryPath, FakeDirectory> _directories;
        private readonly Dictionary<FilePath, FakeFile> _files;

        public bool IsCaseSensitive
        {
            get { return false; }
        }

        public Dictionary<DirectoryPath, FakeDirectory> Directories
        {
            get { return _directories; }
        }

        public Dictionary<FilePath, FakeFile> Files
        {
            get { return _files; }
        }

        public FakeFileSystem()
        {
            _directories = new Dictionary<DirectoryPath, FakeDirectory>(new PathComparer(IsCaseSensitive));
            _files = new Dictionary<FilePath, FakeFile>(new PathComparer(IsCaseSensitive));
        }

        public IFile GetFile(FilePath path)
        {
            if (!Files.ContainsKey(path))
            {
                Files.Add(path, new FakeFile(path));
            }
            return Files[path];
        }

        public IFile GetCreatedFile(FilePath path)
        {
            var file = GetFile(path);
            file.Open(FileMode.Create, FileAccess.Write, FileShare.None).Close();
            return file;
        }

        public void DeleteDirectory(DirectoryPath path)
        {
            if (Directories.ContainsKey(path))
            {
                Directories[path].Exists = false;
            }
        }

        public IDirectory GetDirectory(DirectoryPath path)
        {
            return GetDirectory(path, creatable: true);
        }

        public IDirectory GetCreatedDirectory(DirectoryPath path)
        {
            var directory = GetDirectory(path, creatable: true);
            directory.Create();
            return directory;
        }

        public IDirectory GetNonCreatableDirectory(DirectoryPath path)
        {
            return GetDirectory(path, creatable: false);
        }

        private IDirectory GetDirectory(DirectoryPath path, bool creatable)
        {
            if (!Directories.ContainsKey(path))
            {
                Directories.Add(path, new FakeDirectory(this, path, creatable));
            }
            return Directories[path];
        }
    }
}