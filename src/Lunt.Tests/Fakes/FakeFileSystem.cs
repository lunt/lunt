// ﻿
// Copyright (c) 2013 Patrik Svensson
// 
// This file is part of Lunt.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
using System.Collections.Generic;
using System.IO;
using Lunt.IO;

namespace Lunt.Tests.Fakes
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
            _directories = new Dictionary<DirectoryPath, FakeDirectory>(new PathComparer(this.IsCaseSensitive));
            _files = new Dictionary<FilePath, FakeFile>(new PathComparer(this.IsCaseSensitive));
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
            var file = this.GetFile(path);
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
            return this.GetDirectory(path, creatable: true);
        }

        public IDirectory GetCreatedDirectory(DirectoryPath path)
        {
            var directory = this.GetDirectory(path, creatable: true);
            directory.Create();
            return directory;
        }

        public IDirectory GetNonCreatableDirectory(DirectoryPath path)
        {
            return this.GetDirectory(path, creatable: false);
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