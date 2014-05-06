using System;
using System.Collections.Generic;
using Xunit;

namespace Lunt.Tests.Unit
{
    public class AssetDefinitionTests
    {
        public class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new AssetDefinition(null, new Dictionary<string, string>()));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("path", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Have_Non_Null_Metadata_Reference_When_No_Metadata_Was_Passed()
            {
                // Given, When
                var result = new AssetDefinition("input/simple.asset");

                // Then
                Assert.NotNull(result.Metadata);
            }

            [Fact]
            public void Should_Consider_Path_A_Glob_If_It_Contains_Directory_Wildcard()
            {
                // Given, When
                var result = new AssetDefinition("input/**/simple.asset");

                // Then
                Assert.True(result.IsGlob);
            }

            [Fact]
            public void Should_Consider_Path_A_Glob_If_It_Contains_Character_Wildcard()
            {
                // Given, When
                var result = new AssetDefinition("input/si?ple.asset");

                // Then
                Assert.True(result.IsGlob);
            }

            [Fact]
            public void Should_Not_Consider_Path_A_Glob_If_It_Does_not_Contain_Wildcards()
            {
                // Given, When
                var result = new AssetDefinition("input/simple.asset");

                // Then
                Assert.False(result.IsGlob);
            }
        }
    }
}
