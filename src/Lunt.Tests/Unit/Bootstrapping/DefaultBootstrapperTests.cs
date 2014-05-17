using Lunt.Bootstrapping;
using Xunit;

namespace Lunt.Tests.Unit.Bootstrapping
{
    public sealed class DefaultBootstrapperTests
    {
        public sealed class TheGetKernelMethod
        {
            [Fact]
            public void Should_Return_Bootstrapped_Build_Kernel()
            {
                // Given
                var bootstrapper = new DefaultBootstrapper();

                // When
                var engine = bootstrapper.GetService<IBuildKernel>();

                // Then
                Assert.IsType<BuildKernel>(engine);
            }
        }
    }
}
