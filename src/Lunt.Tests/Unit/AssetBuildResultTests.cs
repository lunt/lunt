using System;
using Xunit;

namespace Lunt.Tests.Unit
{
    public class AssetBuildResultTests
    {
        public class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Manifest_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new AssetBuildResult(null, string.Empty));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("manifestItem", ((ArgumentNullException) result).ParamName);
            }
        }

        [Fact]
        public void Can_Get_Asset()
        {
            // Given
            var asset = new Asset("asset.txt");
            var item = new BuildManifestItem(asset);

            // When
            var result = new AssetBuildResult(item, string.Empty);

            // Then
            Assert.Equal(asset, result.Asset);
        }
    }
}