using Xunit;

namespace Lunt.Tests.Unit
{
    public class BuildConfigurationTests
    {
        public class TheConstructor
        {
            [Fact]
            public void Should_Create_Asset_Collection()
            {
                // Given, When
                var definition = new BuildConfiguration();

                // Then
                Assert.NotNull(definition.Assets);
            }
        }
    }
}