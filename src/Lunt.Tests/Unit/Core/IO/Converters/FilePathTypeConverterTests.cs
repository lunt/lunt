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
using Lunt.IO;
using Xunit;

namespace Lunt.Tests.Unit.IO.Converters
{
    public class FilePathTypeConverterTests
    {
        [Fact]
        public void Can_Get_Converter()
        {
            // Given, When
            var converter = TypeDescriptor.GetConverter(typeof (FilePath));

            // Then
            Assert.NotNull(converter);
            Assert.Equal("Lunt.IO.FilePathTypeConverter", converter.GetType().FullName);
        }

        [Fact]
        public void Can_Convert_From_String()
        {
            // Given
            var converter = TypeDescriptor.GetConverter(typeof (FilePath));

            // When
            var result = converter.CanConvertFrom(typeof (string));

            // Then
            Assert.True(result);
        }

        [Fact]
        public void Can_Perform_Convertion_From_String()
        {
            // Given
            var converter = TypeDescriptor.GetConverter(typeof (FilePath));

            // When
            var result = converter.ConvertFromString("/hello/world") as FilePath;

            // Then
            Assert.NotNull(result);
            Assert.Equal("/hello/world", result.FullPath);
        }

        [Fact]
        public void Should_Not_Indicate_That_Non_Strings_Can_Be_Converted()
        {
            // Given
            var converter = TypeDescriptor.GetConverter(typeof (FilePath));

            // When
            var result = converter.CanConvertFrom(typeof (int));

            // Then
            Assert.False(result);
        }

        [Fact]
        public void Should_Throw_When_Converting_From_Non_String()
        {
            // Given
            var converter = TypeDescriptor.GetConverter(typeof (FilePath));

            // When
            var result = Record.Exception(() => converter.ConvertFrom(new List<string>()));

            // Then
            Assert.IsType<NotSupportedException>(result);
        }
    }
}