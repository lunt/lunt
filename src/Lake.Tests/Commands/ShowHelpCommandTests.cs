using System;
using Lake.Commands;
using Lunt.Testing;
using Xunit;

namespace Lake.Tests.Unit.Commands
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
                Assert.True(console.Content[0].StartsWith("Usage: Lake.exe"));
            }
        }
    }
}