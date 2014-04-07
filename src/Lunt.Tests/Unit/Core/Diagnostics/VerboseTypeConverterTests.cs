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
using System.ComponentModel;
using Lunt.Diagnostics;
using Xunit;
using Xunit.Extensions;

namespace Lunt.Tests.Unit.Diagnostics
{
    public class VerboseTypeConverterTests
    {
        [Theory]
        [InlineData("q", Verbosity.Quiet)]
        [InlineData("quiet", Verbosity.Quiet)]
        [InlineData("m", Verbosity.Minimal)]
        [InlineData("minimal", Verbosity.Minimal)]
        [InlineData("n", Verbosity.Normal)]
        [InlineData("normal", Verbosity.Normal)]
        [InlineData("v", Verbosity.Verbose)]
        [InlineData("verbose", Verbosity.Verbose)]
        [InlineData("d", Verbosity.Diagnostic)]
        [InlineData("diagnostic", Verbosity.Diagnostic)]
        [InlineData("Q", Verbosity.Quiet)]
        [InlineData("Quiet", Verbosity.Quiet)]
        [InlineData("M", Verbosity.Minimal)]
        [InlineData("Minimal", Verbosity.Minimal)]
        [InlineData("N", Verbosity.Normal)]
        [InlineData("Normal", Verbosity.Normal)]
        [InlineData("V", Verbosity.Verbose)]
        [InlineData("Verbose", Verbosity.Verbose)]
        [InlineData("D", Verbosity.Diagnostic)]
        [InlineData("Diagnostic", Verbosity.Diagnostic)]
        public void Can_Convert_From_String(string input, Verbosity expected)
        {
            // Given
            var converter = TypeDescriptor.GetConverter(typeof (Verbosity));

            // When
            var result = converter.ConvertFromString(input);

            // Then
            Assert.IsType<Verbosity>(result);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Should_Indicate_That_Strings_Can_Be_Converted()
        {
            // Given
            var converter = TypeDescriptor.GetConverter(typeof (Verbosity));

            // When
            var result = converter.CanConvertFrom(typeof (string));

            // Then
            Assert.True(result);
        }

        [Fact]
        public void Should_Not_Indicate_That_Non_Strings_Can_Be_Converted()
        {
            // Given
            var converter = TypeDescriptor.GetConverter(typeof (Verbosity));

            // When
            var result = converter.CanConvertFrom(typeof (int));

            // Then
            Assert.False(result);
        }

        [Fact]
        public void Should_Throw_When_Converting_From_Non_String()
        {
            // Given
            var converter = TypeDescriptor.GetConverter(typeof (Verbosity));

            // When
            var result = Record.Exception(() => converter.ConvertFrom(new List<string>()));

            // Then
            Assert.IsType<NotSupportedException>(result);
        }

        [Fact]
        public void Should_Throw_When_Converting_From_Non_Known_String()
        {
            // Given
            var converter = TypeDescriptor.GetConverter(typeof (Verbosity));

            // When
            var result = Record.Exception(() => converter.ConvertFromString("Hello"));

            // Then
            Assert.IsType<NotSupportedException>(result);
        }
    }
}