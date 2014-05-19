using Lunt;
using Lunt.Bootstrapping;
using Lunt.Testing;
using Lunt.Tests.Framework;
using Xunit;

namespace Lake.Tests
{
    public sealed class LakeInternalConfigurationTests
    {
        [Fact]
        public void Can_Resolve_Build_Kernel_With_Configuration()
        {
            // Given
            var config = new LakeInternalConfiguration(
                new FakeBuildLog(), new FakeBuildEnvironment(), new FakePipelineScanner());

            // When
            var kernel = new DefaultBootstrapper(config).GetService<IBuildKernel>();

            // Then
            Assert.IsType<BuildKernel>(kernel);
        }
    }
}
