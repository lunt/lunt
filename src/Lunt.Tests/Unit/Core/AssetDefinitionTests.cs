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
using System.Collections.Generic;
using Xunit;

namespace Lunt.Tests.Unit.Core
{
    public class AssetDefinitionTests
    {
        public class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new AssetDefinition(null, new Dictionary<string, string>()));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("path", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Have_Non_Null_Metadata_Reference_When_No_Metadata_Was_Passed()
            {
                // Given, When
                var result = new AssetDefinition("input/simple.asset");

                // Then
                Assert.NotNull(result.Metadata);
            }

            [Fact]
            public void Should_Consider_Path_A_Glob_If_It_Contains_Directory_Wildcard()
            {
                // Given, When
                var result = new AssetDefinition("input/**/simple.asset");

                // Then
                Assert.True(result.IsGlob);
            }

            [Fact]
            public void Should_Consider_Path_A_Glob_If_It_Contains_Character_Wildcard()
            {
                // Given, When
                var result = new AssetDefinition("input/si?ple.asset");

                // Then
                Assert.True(result.IsGlob);
            }

            [Fact]
            public void Should_Not_Consider_Path_A_Glob_If_It_Does_not_Contain_Wildcards()
            {
                // Given, When
                var result = new AssetDefinition("input/simple.asset");

                // Then
                Assert.False(result.IsGlob);
            }
        }
    }
}
