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
using Lunt.IO;

namespace Lunt.Tests.Fakes
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
