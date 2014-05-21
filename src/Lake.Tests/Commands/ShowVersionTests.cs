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
                var options = new LakeOptions();

                // When
                var result = new ShowVersionCommand(console).Execute(options);

                // Then
                Assert.Equal(0, result);
                Assert.True(console.Content[0].StartsWith(" - Lake.exe"));
                Assert.True(console.Content[1].StartsWith(" - Lunt.dll"));
            }
        }
    }
}