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
                var result = Record.Exception(() => new AssetBuildResult(null, Message.Empty));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("manifestItem", ((ArgumentNullException)result).ParamName);
            }
        }

        public class TheAssetProperty
        {
            [Fact]
            public void Should_Return_The_Asset_Provided_In_The_Constructor()
            {
                // Given
                var asset = new Asset("asset.txt");
                var item = new BuildManifestItem(asset);

                // When
                var result = new AssetBuildResult(item, Message.Empty);

                // Then
                Assert.Equal(asset, result.Asset);
            }
        }

        public class TheMessageProperty
        {
            [Fact]
            public void Should_Return_The_Message_Provided_In_The_Constructor()
            {
                // Given
                var item = new BuildManifestItem(new Asset("asset.txt"));
                var message = new Message("Hello World");

                // When
                var result = new AssetBuildResult(item, message);

                // Then
                Assert.Equal(message, result.Message);
            }
        }
    }
}