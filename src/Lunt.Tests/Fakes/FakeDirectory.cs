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
using System.Linq;
using Lunt.IO;

namespace Lunt.Tests.Fakes
{
    public class FakeDirectory : IDirectory
    {
        private readonly FakeFileSystem _fileSystem;
        private readonly DirectoryPath _path;
        private readonly bool _creatable;
        private bool _exist;

        public DirectoryPath Path
        {
            get { return _path; }
        }

        public bool Exists
        {
            get { return _exist; }
            set { _exist = value; }
        }

        public FakeDirectory(FakeFileSystem fileSystem, DirectoryPath path, bool creatable)
        {
            _fileSystem = fileSystem;
            _path = path;
            _exist = false;
            _creatable = creatable;
        }

        public virtual bool Create()
        {
            if (_creatable)
            {
                _exist = true;
            }
            return _creatable;
        }

        public IEnumerable<IDirectory> GetDirectories(string filter, SearchScope scope)
        {
            var result = new List<IDirectory>();
            var children = _fileSystem.Directories.Where(x => x.Key.FullPath.StartsWith(_path.FullPath + "/"));
            foreach (var child in children)
            {
                var relative = child.Key.FullPath.Substring(_path.FullPath.Length + 1);
                if (!relative.Contains("/"))
                {
                    result.Add(child.Value);
                }
            }
            return result;
        }

        public IEnumerable<IFile> GetFiles(string filter, SearchScope scope)
        {
            var result = new List<IFile>();
            var children = _fileSystem.Files.Where(x => x.Key.FullPath.StartsWith(_path.FullPath + "/"));
            foreach (var child in children)
            {
                var relative = child.Key.FullPath.Substring(_path.FullPath.Length + 1);
                if (!relative.Contains("/"))
                {
                    result.Add(child.Value);
                }
            }
            return result;
        }
    }
}