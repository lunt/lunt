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
using System;
using System.IO;
using Lunt.IO;

namespace Lunt.Tests.Fakes
{
    public class FakeFile : IFile
    {
        private readonly FilePath _path;
        private bool _exists;
        private byte[] _content = new byte[4096];
        private long _contentLength;
        private readonly object _contentLock = new object();

        public FilePath Path
        {
            get { return _path; }
        }

        public bool Exists
        {
            get { return _exists; }
        }

        public long Length
        {
            get { return _contentLength; }
        }

        public object ContentLock
        {
            get { return _contentLock; }
        }

        public long ContentLength
        {
            get { return _contentLength; }
            set { _contentLength = value; }
        }

        public byte[] Content
        {
            get { return _content; }
            set { _content = value; }
        }

        public FakeFile(FilePath path)
        {
            _path = path;
            _exists = false;
        }

        public Stream Open(FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            var position = this.GetPosition(fileMode);
            var stream = new FakeFileStream(this);
            stream.Position = position;
            return stream;
        }

        public void Resize(long offset)
        {
            if (_contentLength < offset)
            {
                _contentLength = offset;
            }
            if (_content.Length >= _contentLength)
            {
                return;
            }

            var buffer = new byte[_contentLength*2];
            Buffer.BlockCopy(_content, 0, buffer, 0, _content.Length);
            _content = buffer;
        }

        private long GetPosition(FileMode fileMode)
        {
            if (Exists)
            {
                switch (fileMode)
                {
                    case FileMode.CreateNew:
                        throw new IOException();
                    case FileMode.Create:
                    case FileMode.Truncate:
                        _contentLength = 0;
                        return 0;
                    case FileMode.Open:
                    case FileMode.OpenOrCreate:
                        return 0;
                    case FileMode.Append:
                        return _contentLength;
                }
            }
            else
            {
                switch (fileMode)
                {
                    case FileMode.Create:
                    case FileMode.Append:
                    case FileMode.CreateNew:
                    case FileMode.OpenOrCreate:
                        _exists = true;
                        return _contentLength;
                    case FileMode.Open:
                    case FileMode.Truncate:
                        throw new FileNotFoundException();
                }
            }
            throw new NotSupportedException();
        }
    }
}