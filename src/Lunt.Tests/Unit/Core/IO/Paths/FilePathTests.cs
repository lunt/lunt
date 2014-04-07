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
using Xunit;
using Xunit.Extensions;

namespace Lunt.Tests.Unit.IO.Paths
{
    public class FilePathTests
    {
        [Theory]
        [InlineData("assets/shaders/basic.txt", true)]
        [InlineData("assets/shaders/basic", false)]
        [InlineData("assets/shaders/basic/", false)]
        public void Can_See_If_A_Path_Has_An_Extension(string fullPath, bool expected)
        {
            // Given, When
            var path = new FilePath(fullPath);

            // Then
            Assert.Equal(expected, path.HasExtension);
        }

        [Theory]
        [InlineData("assets/shaders/basic.frag", ".frag")]
        [InlineData("assets/shaders/basic.frag/test.vert", ".vert")]
        [InlineData("assets/shaders/basic", null)]
        [InlineData("assets/shaders/basic.frag/test", null)]
        public void Can_Get_Extension(string fullPath, string expected)
        {
            // Given, When
            var result = new FilePath(fullPath);
            var extension = result.GetExtension();

            // Then
            Assert.Equal(expected, extension);
        }

        [Fact]
        public void Can_Get_Directory_For_File_Path()
        {
            // Given, When
            var path = new FilePath("temp/hello.txt");
            var directory = path.GetDirectory();

            // Then
            Assert.Equal("temp", directory.FullPath);
        }

        [Fact]
        public void Can_Get_Directory_For_File_Path_In_Root()
        {
            // Given, When
            var path = new FilePath("hello.txt");
            var directory = path.GetDirectory();

            // Then
            Assert.Equal(string.Empty, directory.FullPath);
        }

        [Fact]
        public void Can_Change_Extension_Of_Path()
        {
            // Given
            var path = new FilePath("temp/hello.txt");

            // When
            path = path.ChangeExtension(".dat");

            // Then
            Assert.Equal("temp/hello.dat", path.ToString());
        }

        [Fact]
        public void Can_Get_Filename_From_Path()
        {
            // Given
            var path = new FilePath("/input/test.txt");

            // When
            var result = path.GetFilename();

            // Then
            Assert.Equal("test.txt", result.FullPath);
        }
    }
}