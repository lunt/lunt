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
using Lunt.Tests.Fakes;

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
