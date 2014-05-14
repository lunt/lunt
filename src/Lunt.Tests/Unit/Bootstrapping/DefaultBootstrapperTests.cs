using Lunt.Bootstrapping;
using Xunit;

namespace Lunt.Tests.Unit.Bootstrapping
{
    public class DefaultBootstrapperTests
    {
        public class TheGetKernelMethod
        {
            [Fact]
            public void Should_Throw_If_Initialize_Has_Not_Been_Called()
            {
                // Given
                using (var bootstrapper = new DefaultBootstrapper())
                {
                    // When, Then
                    var exception = Assert.Throws<LuntException>(() => bootstrapper.GetKernel());
                    Assert.Equal("Bootstrapper have not been initialized.", exception.Message);
                }
            }

            [Fact]
            public void Should_Return_Bootstrapped_Build_Kernel()
            {
                // Given
                using (var bootstrapper = new DefaultBootstrapper())
                {
                    bootstrapper.Initialize();

                    // When
                    var kernel = bootstrapper.GetKernel();

                    // Then
                    Assert.IsType<BuildKernel>(kernel);
                }
            }
        }
    }
}
