using Lake.Diagnostics;
using Lunt;
using Lunt.Diagnostics;
using Moq;
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
                var writer = new Mock<IConsoleWriter>();
                var log = new LakeBuildLog(writer.Object);

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
                var writer = new Mock<IConsoleWriter>();
                var log = new LakeBuildLog(writer.Object);

                // When
                log.Write(Verbosity.Normal, LogLevel.Information, "Hello World");

                // Then
                writer.Verify(x => x.WriteLine("[I] Hello World"));
            }
        }
    }
}
