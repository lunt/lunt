using System;
using Lunt.IO;
using Xunit;

namespace Lunt.Tests.Unit
{
    public class AssetDependencyTests
    {
        public class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new AssetDependency(null, 12, "ABC"));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("path", ((ArgumentNullException) result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Checksum_Is_Null()
            {
                // Given, When
                var path = new FilePath("simple.asset");
                var result = Record.Exception(() => new AssetDependency(path, 12, null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("checksum", ((ArgumentNullException) result).ParamName);
            }
        }
    }
}