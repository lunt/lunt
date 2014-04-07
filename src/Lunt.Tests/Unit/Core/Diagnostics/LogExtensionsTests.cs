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
using Lunt.Diagnostics;
using Xunit;

namespace Lunt.Tests.Unit.Diagnostics
{
    public class LogExtensionsTests
    {
        private class TestLogger : IBuildLog
        {
            private Verbosity _verbosity;
            private LogLevel _level;
            private string _message;

            public Verbosity Verbosity
            {
                get { return _verbosity; }
            }

            public LogLevel Level
            {
                get { return _level; }
            }

            public string Message
            {
                get { return _message; }
            }

            public void Write(Verbosity verbosity, LogLevel level, string message)
            {
                _verbosity = verbosity;
                _level = level;
                _message = message;
            }
        }

        public class TheDebugMethod
        {
            [Fact]
            public void Should_Not_Throw_If_Log_Is_Null_When_Logging_With_Default_Verbosity()
            {
                // Given, When
                var result = Record.Exception(() => LogExtensions.Debug(null, "Hello World"));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Should_Not_Throw_If_Log_Is_Null_When_Logging_With_Custom_Verbosity()
            {
                // Given, When
                var result = Record.Exception(() => LogExtensions.Debug(null, Verbosity.Normal, "Hello World"));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Can_Write_Debug_Message_With_Default_Verbosity()
            {
                // Given
                var logger = new TestLogger();

                // When
                logger.Debug("Hello World");

                // Then
                Assert.Equal(Verbosity.Diagnostic, logger.Verbosity);
                Assert.Equal(LogLevel.Debug, logger.Level);
                Assert.Equal("Hello World", logger.Message);
            }

            [Fact]
            public void Can_Write_Debug_Message_With_Custom_Verbosity()
            {
                // Given
                var logger = new TestLogger();

                // When
                logger.Debug(Verbosity.Quiet, "Hello World");

                // Then
                Assert.Equal(Verbosity.Quiet, logger.Verbosity);
                Assert.Equal(LogLevel.Debug, logger.Level);
                Assert.Equal("Hello World", logger.Message);
            }
        }

        public class TheVerboseMethod
        {
            [Fact]
            public void Should_Not_Throw_If_Log_Is_Null_When_Logging_With_Default_Verbosity()
            {
                // Given, When
                var result = Record.Exception(() => LogExtensions.Verbose(null, "Hello World"));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Should_Not_Throw_If_Log_Is_Null_When_Logging_With_Custom_Verbosity()
            {
                // Given, When
                var result = Record.Exception(() => LogExtensions.Verbose(null, Verbosity.Normal, "Hello World"));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Can_Write_Verbose_Message_With_Default_Verbosity()
            {
                // Given
                var logger = new TestLogger();

                // When
                logger.Verbose("Hello World");

                // Then
                Assert.Equal(Verbosity.Verbose, logger.Verbosity);
                Assert.Equal(LogLevel.Verbose, logger.Level);
                Assert.Equal("Hello World", logger.Message);
            }

            [Fact]
            public void Can_Write_Verbose_Message_With_Custom_Verbosity()
            {
                // Given
                var logger = new TestLogger();

                // When
                logger.Verbose(Verbosity.Quiet, "Hello World");

                // Then
                Assert.Equal(Verbosity.Quiet, logger.Verbosity);
                Assert.Equal(LogLevel.Verbose, logger.Level);
                Assert.Equal("Hello World", logger.Message);
            }
        }

        public class TheInformationMethod
        {
            [Fact]
            public void Should_Not_Throw_If_Log_Is_Null_When_Logging_With_Default_Verbosity()
            {
                // Given, When
                var result = Record.Exception(() => LogExtensions.Information(null, "Hello World"));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Should_Not_Throw_If_Log_Is_Null_When_Logging_With_Custom_Verbosity()
            {
                // Given, When
                var result = Record.Exception(() => LogExtensions.Information(null, Verbosity.Normal, "Hello World"));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Can_Write_Informative_Message_With_Default_Verbosity()
            {
                // Given
                var logger = new TestLogger();

                // When
                logger.Information("Hello World");

                // Then
                Assert.Equal(Verbosity.Normal, logger.Verbosity);
                Assert.Equal(LogLevel.Information, logger.Level);
                Assert.Equal("Hello World", logger.Message);
            }

            [Fact]
            public void Can_Write_Informative_Message_With_Custom_Verbosity()
            {
                // Given
                var logger = new TestLogger();

                // When
                logger.Information(Verbosity.Quiet, "Hello World");

                // Then
                Assert.Equal(Verbosity.Quiet, logger.Verbosity);
                Assert.Equal(LogLevel.Information, logger.Level);
                Assert.Equal("Hello World", logger.Message);
            }
        }

        public class TheWarningMethod
        {
            [Fact]
            public void Should_Not_Throw_If_Log_Is_Null_When_Logging_With_Default_Verbosity()
            {
                // Given, When
                var result = Record.Exception(() => LogExtensions.Warning(null, "Hello World"));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Should_Not_Throw_If_Log_Is_Null_When_Logging_With_Custom_Verbosity()
            {
                // Given, When
                var result = Record.Exception(() => LogExtensions.Warning(null, Verbosity.Normal, "Hello World"));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Can_Write_Warning_Message_With_Default_Verbosity()
            {
                // Given
                var logger = new TestLogger();

                // When
                logger.Warning("Hello World");

                // Then
                Assert.Equal(Verbosity.Minimal, logger.Verbosity);
                Assert.Equal(LogLevel.Warning, logger.Level);
                Assert.Equal("Hello World", logger.Message);
            }

            [Fact]
            public void Can_Write_Warning_Message_With_Custom_Verbosity()
            {
                // Given
                var logger = new TestLogger();

                // When
                logger.Warning(Verbosity.Quiet, "Hello World");

                // Then
                Assert.Equal(Verbosity.Quiet, logger.Verbosity);
                Assert.Equal(LogLevel.Warning, logger.Level);
                Assert.Equal("Hello World", logger.Message);
            }
        }

        public class TheErrorMethod
        {
            [Fact]
            public void Should_Not_Throw_If_Log_Is_Null_When_Logging_With_Default_Verbosity()
            {
                // Given, When
                var result = Record.Exception(() => LogExtensions.Error(null, "Hello World"));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Should_Not_Throw_If_Log_Is_Null_When_Logging_With_Custom_Verbosity()
            {
                // Given, When
                var result = Record.Exception(() => LogExtensions.Error(null, Verbosity.Normal, "Hello World"));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Can_Write_Error_Message_With_Default_Verbosity()
            {
                // Given
                var logger = new TestLogger();

                // When
                logger.Error("Hello World");

                // Then
                Assert.Equal(Verbosity.Quiet, logger.Verbosity);
                Assert.Equal(LogLevel.Error, logger.Level);
                Assert.Equal("Hello World", logger.Message);
            }

            [Fact]
            public void Can_Write_Error_Message_With_Custom_Verbosity()
            {
                // Given
                var logger = new TestLogger();

                // When
                logger.Error(Verbosity.Diagnostic, "Hello World");

                // Then
                Assert.Equal(Verbosity.Diagnostic, logger.Verbosity);
                Assert.Equal(LogLevel.Error, logger.Level);
                Assert.Equal("Hello World", logger.Message);
            }
        }
    }
}