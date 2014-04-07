﻿// ﻿
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
using Lunt.Commands;
using Lunt.Tests.Fakes;
using Xunit;

namespace Lunt.Tests.Unit.Commands
{
    public class ShowHelpCommandTests
    {
        public class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Console_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new ShowHelpCommand(null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("console", ((ArgumentNullException)result).ParamName);
            }
        }

        public class TheExecuteMethod
        {
            [Fact]
            public void Should_Output_Usage_Information()
            {
                // Given
                var console = new FakeConsole();

                // When
                var result = new ShowHelpCommand(console).Execute();

                // Then
                Assert.Equal(0, result);
                Assert.True(console.Content[0].StartsWith("Usage: Lunt.exe"));
            }
        }
    }
}