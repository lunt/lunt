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
using System.Runtime.Serialization.Formatters.Binary;
using Xunit;

namespace Lunt.Tests.Unit
{
    public class LuntExceptionTests
    {
        public class TheConstructor
        {
            [Fact]
            public void Can_Create_Exception_Using_Default_Constructor()
            {
                // Given, When
                var result = new LuntException();

                // Then
                Assert.Equal("Exception of type 'Lunt.LuntException' was thrown.", result.Message);
            }

            [Fact]
            public void Can_Create_Exception_With_Message()
            {
                // Given, When
                var result = new LuntException("Hello World");

                // Then
                Assert.Equal("Hello World", result.Message);
            }

            [Fact]
            public void Can_Create_Exception_With_Message_And_Exception()
            {
                // Given
                var exception = new NotImplementedException("Hello World");

                // When
                var result = new LuntException("Hello World", exception);

                // Then
                Assert.Equal("Hello World", result.Message);
                Assert.Equal(exception, result.InnerException);
            }
        }

        [Fact]
        public void Can_Serialize_Lunt_Exception()
        {
            // Given
            var obj = new LuntException("Hello World");
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, obj);
                stream.Seek(0, SeekOrigin.Begin);

                // When
                var result = formatter.Deserialize(stream) as LuntException;

                // Then
                Assert.NotNull(result);
                Assert.Equal("Hello World", result.Message);
            }
        }
    }
}