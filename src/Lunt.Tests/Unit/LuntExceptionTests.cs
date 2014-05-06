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