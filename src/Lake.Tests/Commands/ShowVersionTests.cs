using System;
using Lake.Commands;
using Lunt.Testing;
using Xunit;

namespace Lake.Tests.Unit.Commands
{
    public class ShowVersionTests
    {
        public class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Console_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new ShowVersionCommand(null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("console", ((ArgumentNullException) result).ParamName);
            }
        }

        public class TheExecuteMethod
        {
            [Fact]
            public void Should_Write_Version_Information_To_The_Console()
            {
                // Given
                var console = new FakeConsole();

                // When
                var result = new ShowVersionCommand(console).Execute();

                // Then
                Assert.Equal(0, result);
                Assert.Equal(2, console.Content.Count);
            }
        }
    }
}