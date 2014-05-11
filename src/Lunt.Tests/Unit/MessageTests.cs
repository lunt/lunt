using System;
using Xunit;

namespace Lunt.Tests.Unit
{
    public sealed class MessageTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_The_Format_Is_Null()
            {
                // Given, When, Then
                var exception = Record.Exception(() => new Message(null));
                
                // Then
                Assert.IsType<ArgumentNullException>(exception);
                Assert.Equal("format", ((ArgumentNullException)exception).ParamName);
            }

            [Fact]
            public void Should_Throw_If_The_Arguments_Are_Null()
            {
                // Given, When, Then
                var exception = Record.Exception(() => new Message("Hello", null));

                // Then
                Assert.IsType<ArgumentNullException>(exception);
                Assert.Equal("args", ((ArgumentNullException)exception).ParamName);
            }
        }

        public sealed class TheFormatProperty
        {
            [Fact]
            public void Should_Return_The_Format_Provided_In_The_Constructor()
            {
                // Given, When
                var message = new Message("Hello");

                // Then
                Assert.Equal("Hello", message.Format);
            }
        }

        public sealed class TheArgumentsProperty
        {
            [Fact]
            public void Should_Return_The_Arguments_Provided_In_The_Constructor()
            {
                // Given, When
                var message = new Message("Hello", "World");

                // Then
                Assert.Equal(new object[] { "World" }, message.Arguments);
            }
        }

        public sealed class TheToStringMethod
        {
            [Fact]
            public void Should_Convert_The_Message_To_A_String()
            {
                // Given, When
                var message = new Message("Hello {0}", "World");

                // Then
                Assert.Equal("Hello World", message.ToString());
            }
        }
    }
}
