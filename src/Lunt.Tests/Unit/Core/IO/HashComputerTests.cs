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
using Lunt.IO;
using Lunt.Tests.Extensions;
using Xunit;

namespace Lunt.Tests.Unit.IO
{
    public class HashComputerTests
    {
        [Fact]
        public void Disposed_Instance_Should_Throw_If_Asked_To_Compute_Hash()
        {
            // Given
            var hasher = new HashComputer();
            var filesystem = new FileSystem();
            var file = filesystem.GetFile("file.data").Create("Hello World");
            hasher.Dispose();

            // When
            var result = Record.Exception(() => hasher.Compute(file));

            // Then
            Assert.IsType<ObjectDisposedException>(result);
            Assert.Equal("Lunt.IO.HashComputer", ((ObjectDisposedException) result).ObjectName);
        }

        [Fact]
        public void Same_Input_Returns_The_Same_Hashes()
        {
            // Given
            var hasher = new HashComputer();
            var filesystem = new FileSystem();
            var file = filesystem.GetFile("file.data").Create("Hello World");

            // When
            string first = hasher.Compute(file);
            string second = hasher.Compute(file);

            // Then
            Assert.Equal(first, second);
        }

        [Fact]
        public void Different_Input_Returns_Different_Hashes()
        {
            // Given
            var hasher = new HashComputer();
            var filesystem = new FileSystem();
            var file1 = filesystem.GetFile("file1.data").Create("Hello World");
            var file2 = filesystem.GetFile("file2.data").Create("Goodbye World");

            // When
            string first = hasher.Compute(file1);
            string second = hasher.Compute(file2);

            // Then
            Assert.NotEqual(first, second);
        }
    }
}