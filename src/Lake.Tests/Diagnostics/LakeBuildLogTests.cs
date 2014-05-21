using Lake.Diagnostics;
using Lunt;
using Lunt.Diagnostics;
using NSubstitute;
using Xunit;

namespace Lake.Tests.Diagnostics
{
    public sealed class LakeBuildLogTests
    {
        public sealed class TheVerbosityProperty
        {
            [Fact]
            public void Should_Default_To_Diagnostic_Verbosity()
            {
                // Given, When
                var writer = Substitute.For<IConsoleWriter>();
                var log = new LakeBuildLog(writer);

                // Then
                Assert.Equal(Verbosity.Diagnostic, log.Verbosity);
            }
        }

        public sealed class TheWriteMethod
        {
            [Fact]
            public void Should_Prefix_The_Log_Message_With_The_Log_Level()
            {
                // Given
                var writer = Substitute.For<IConsoleWriter>();
                var log = new LakeBuildLog(writer);

                // When
                log.Write(Verbosity.Normal, LogLevel.Information, "Hello World");

                // Then
                writer.Received(1).Write("[I] Hello World");
            }
        }
    }
}
