using System;
using System.Collections.Generic;
using Xunit;

namespace Lunt.Tests.Unit
{
    public class AssetMetadataTests
    {
        public class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Dictionary_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new AssetMetadata(null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("dictionary", ((ArgumentNullException) result).ParamName);
            }

            [Fact]
            public void Can_Get_Defined_Value()
            {
                // Given
                var dictionary = new Dictionary<string, string>();
                dictionary.Add("key", "value");

                // When
                var result = new AssetMetadata(dictionary);

                // Then
                Assert.Equal("value", result.GetValue("key"));
            }

            [Fact]
            public void Returns_Null_For_Non_Existing_Value()
            {
                // Given
                var dictionary = new Dictionary<string, string>();

                // When
                var result = new AssetMetadata(dictionary);

                // Then
                Assert.Null(result.GetValue("key"));
            }

            [Fact]
            public void Can_See_If_Key_Exist()
            {
                // Given
                var dictionary = new Dictionary<string, string>();
                dictionary.Add("key", "value");

                // When
                var result = new AssetMetadata(dictionary);

                // Then
                Assert.True(result.IsDefined("key"));
            }

            [Fact]
            public void Can_Get_Number_Of_Keys()
            {
                // Given
                var dictionary = new Dictionary<string, string>();
                dictionary.Add("key", "value");

                // When
                var result = new AssetMetadata(dictionary);

                // Then
                Assert.Equal(1, result.Count);
            }

            [Fact]
            public void Can_Not_See_Key_That_Does_Not_Exist()
            {
                // Given
                var dictionary = new Dictionary<string, string>();
                dictionary.Add("key", "value");

                // When
                var result = new AssetMetadata(dictionary);

                // Then
                Assert.False(result.IsDefined("key2"));
            }

            [Fact]
            public void Keys_Are_Not_Case_Sensitive()
            {
                // Given
                var dictionary = new Dictionary<string, string>();
                dictionary.Add("key", "value");

                // When
                var result = new AssetMetadata(dictionary);

                // Then
                Assert.True(result.IsDefined("KEY"));
                Assert.Equal("value", result.GetValue("KEY"));
            }
        }
    }
}