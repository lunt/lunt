using System;
using Xunit;

namespace Lunt.Tests.Unit
{
    public class BuildManifestItemTests
    {
        public class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Asset_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new BuildManifestItem(null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("asset", ((ArgumentNullException) result).ParamName);
            }
        }
    }
}