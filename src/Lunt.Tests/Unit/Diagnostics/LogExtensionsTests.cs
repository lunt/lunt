using Lunt.Diagnostics;
using Xunit;

namespace Lunt.Tests.Unit.Diagnostics
{
    public class LogExtensionsTests
    {
        private sealed class TestingLogger : IBuildLog
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

            public void Write(Verbosity verbosity, LogLevel level, string format, params object[] args)
            {
                _verbosity = verbosity;
                _level = level;
                _message = string.Format(format, args);
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
                var logger = new TestingLogger();

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
                var logger = new TestingLogger();

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
                var logger = new TestingLogger();

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
                var logger = new TestingLogger();

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
                var logger = new TestingLogger();

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
                var logger = new TestingLogger();

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
                var logger = new TestingLogger();

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
                var logger = new TestingLogger();

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
                var logger = new TestingLogger();

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
                var logger = new TestingLogger();

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