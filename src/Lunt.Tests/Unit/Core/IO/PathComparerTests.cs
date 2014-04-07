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

namespace Lunt.Tests.Unit.IO
{
    public class PathComparerTests
    {
        public class TheEqualsMethod
        {
            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Same_Asset_Instances_Is_Considered_Equal(bool isCaseSensitive)
            {
                // Given, When
                var comparer = new PathComparer(isCaseSensitive);
                var path = new FilePath("shaders/basic.vert");

                // Then
                Assert.True(comparer.Equals(path, path));
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Two_Null_Paths_Are_Considered_Equal(bool isCaseSensitive)
            {
                // Given
                var comparer = new PathComparer(isCaseSensitive);

                // When
                var result = comparer.Equals(null, null);

                // Then
                Assert.True(result);
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Paths_Are_Considered_Inequal_If_Any_Is_Null(bool isCaseSensitive)
            {
                // Given
                var comparer = new PathComparer(isCaseSensitive);

                // When
                var result = comparer.Equals(null, new FilePath("test.txt"));

                // Then
                Assert.False(result);
            }


            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Same_Paths_Are_Considered_Equal(bool isCaseSensitive)
            {
                // Given, When
                var comparer = new PathComparer(isCaseSensitive);
                var first = new FilePath("shaders/basic.vert");
                var second = new FilePath("shaders/basic.vert");

                // Then
                Assert.True(comparer.Equals(first, second));
                Assert.True(comparer.Equals(second, first));
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Different_Paths_Are_Not_Considered_Equal(bool isCaseSensitive)
            {
                // Given, When
                var comparer = new PathComparer(isCaseSensitive);
                var first = new FilePath("shaders/basic.vert");
                var second = new FilePath("shaders/basic.frag");

                // Then
                Assert.False(comparer.Equals(first, second));
                Assert.False(comparer.Equals(second, first));
            }

            [Theory]
            [InlineData(true, false)]
            [InlineData(false, true)]
            public void Same_Paths_But_Different_Casing_Are_Considered_Equal_Depending_On_Case_Sensitivity(bool isCaseSensitive, bool expected)
            {
                // Given, When
                var comparer = new PathComparer(isCaseSensitive);
                var first = new FilePath("shaders/basic.vert");
                var second = new FilePath("SHADERS/BASIC.VERT");

                // Then
                Assert.Equal(expected, comparer.Equals(first, second));
                Assert.Equal(expected, comparer.Equals(second, first));
            }
        }

        public class TheGetHashCodeMethod
        {
            [Fact]
            public void Should_Throw_If_Other_Path_Is_Null()
            {
                // Given
                var comparer = new PathComparer(true);

                // When
                var result = Record.Exception(() => comparer.GetHashCode(null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("obj", ((ArgumentNullException) result).ParamName);
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Same_Paths_Get_Same_Hash_Code(bool isCaseSensitive)
            {
                // Given, When
                var comparer = new PathComparer(isCaseSensitive);
                var first = new FilePath("shaders/basic.vert");
                var second = new FilePath("shaders/basic.vert");

                // Then
                Assert.Equal(comparer.GetHashCode(first), comparer.GetHashCode(second));
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Different_Paths_Get_Different_Hash_Codes(bool isCaseSensitive)
            {
                // Given, When
                var comparer = new PathComparer(isCaseSensitive);
                var first = new FilePath("shaders/basic.vert");
                var second = new FilePath("shaders/basic.frag");

                // Then
                Assert.NotEqual(comparer.GetHashCode(first), comparer.GetHashCode(second));
            }

            [Theory]
            [InlineData(true, false)]
            [InlineData(false, true)]
            public void Same_Paths_But_Different_Casing_Get_Same_Hash_Code_Depending_On_Case_Sensitivity(bool isCaseSensitive, bool expected)
            {
                // Given, When
                var comparer = new PathComparer(isCaseSensitive);
                var first = new FilePath("shaders/basic.vert");
                var second = new FilePath("SHADERS/BASIC.VERT");

                // Then
                Assert.Equal(expected, comparer.GetHashCode(first) == comparer.GetHashCode(second));
            }
        }
    }
}