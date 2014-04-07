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
using Xunit;
using Xunit.Extensions;

namespace Lunt.Tests.Unit.IO.Paths
{
    public class DirectoryPathTests
    {
        public class TheCombineWithFilePathMethod
        {
            [Theory]
#if !UNIX
            [InlineData("c:/assets/shaders/", "simple.frag", "c:/assets/shaders/simple.frag")]
            [InlineData("c:/", "simple.frag", "c:/simple.frag")]
#endif
            [InlineData("assets/shaders", "simple.frag", "assets/shaders/simple.frag")]
            [InlineData("assets/shaders/", "simple.frag", "assets/shaders/simple.frag")]
            [InlineData("/assets/shaders/", "simple.frag", "/assets/shaders/simple.frag")]
            public void Should_Combine_Paths(string first, string second, string expected)
            {
                // Given
                var path = new DirectoryPath(first);

                // When
                var result = path.Combine(new FilePath(second));

                // Then
                Assert.Equal(expected, result.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given
                var path = new DirectoryPath("assets");

                // When
                var result = Record.Exception(() => path.Combine((FilePath)null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("path", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Can_Not_Combine_Directory_Path_With_Absolute_File_Path()
            {
                // Given
                var path = new DirectoryPath("assets");

                // When
                var result = Record.Exception(() => path.Combine(new FilePath("/other/asset.txt")));

                // Then
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("Cannot combine a directory path with an absolute file path.", result.Message);
            }
        }

        public class TheCombineWithDirectoryPathMethod
        {
            [Theory]
#if !UNIX
            [InlineData("c:/assets/shaders/", "simple", "c:/assets/shaders/simple")]
            [InlineData("c:/", "simple", "c:/simple")]
#endif
            [InlineData("assets/shaders", "simple", "assets/shaders/simple")]
            [InlineData("assets/shaders/", "simple", "assets/shaders/simple")]
            [InlineData("/assets/shaders/", "simple", "/assets/shaders/simple")]
            public void Should_Combine_Paths(string first, string second, string expected)
            {
                // Given
                var path = new DirectoryPath(first);

                // When
                var result = path.Combine(new DirectoryPath(second));

                // Then
                Assert.Equal(expected, result.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given
                var path = new DirectoryPath("assets");

                // When
                var result = Record.Exception(() => path.Combine((DirectoryPath)null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("path", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Can_Not_Combine_Directory_Path_With_Absolute_Directory_Path()
            {
                // Given
                var path = new DirectoryPath("assets");

                // When
                var result = Record.Exception(() => path.Combine(new DirectoryPath("/other/assets")));

                // Then
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("Cannot combine a directory path with an absolute directory path.", result.Message);
            }
        }
    }
}