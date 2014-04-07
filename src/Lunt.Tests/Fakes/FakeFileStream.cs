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

namespace Lunt.Tests.Fakes
{
    public class FakeFileStream : Stream
    {
        private readonly FakeFile _file;
        private long _position;

        public FakeFileStream(FakeFile file)
        {
            _file = file;
            _position = 0;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
        }

        public override long Length
        {
            get
            {
                lock (_file.ContentLock)
                {
                    return _file.ContentLength;
                }
            }
        }

        public override long Position
        {
            get { return _position; }
            set { this.Seek(value, SeekOrigin.Begin); }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            lock (_file.ContentLock)
            {
                var end = _position + count;
                var fileSize = _file.ContentLength;
                long maxLengthToRead = end > fileSize ? fileSize - _position : count;
                Buffer.BlockCopy(_file.Content, (int) _position, buffer, offset, (int) maxLengthToRead);
                _position += maxLengthToRead;
                return (int) maxLengthToRead;
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (origin == SeekOrigin.Begin)
            {
                return MoveTo(offset);
            }
            if (origin == SeekOrigin.Current)
            {
                return MoveTo(_position + offset);
            }
            if (origin == SeekOrigin.End)
            {
                return MoveTo(_file.ContentLength - offset);
            }
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            lock (_file.ContentLock)
            {
                _file.Resize(value);
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            lock (_file.ContentLock)
            {
                var fileSize = _file.ContentLength;
                var endOfWrite = (_position + count);
                if (endOfWrite > fileSize)
                {
                    _file.Resize(endOfWrite);
                }
                Buffer.BlockCopy(buffer, offset, _file.Content, (int) _position, count);
                _position = _position + count;
            }
        }

        private long MoveTo(long offset)
        {
            lock (_file.ContentLock)
            {
                if (offset < 0)
                {
                    throw new InvalidOperationException();
                }
                if (offset > _file.ContentLength)
                {
                    _file.Resize(offset);
                }
                _position = offset;
                return offset;
            }
        }
    }
}