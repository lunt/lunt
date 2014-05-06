using Lunt.Diagnostics;
using Xunit;

namespace Lake.Tests.Unit
{
    public class LakeOptionsTests
    {
        public class TheConstructor
        {
            [Fact]
            public void Should_Have_Verbosity_Set_To_Normal()
            {
                // Given, When
                var result = new LakeOptions();

                // Then
                Assert.Equal(Verbosity.Normal, result.Verbosity);
            }

            [Fact]
            public void Should_Not_Show_Help()
            {
                // Given, When
                var result = new LakeOptions();

                // Then
                Assert.False(result.ShowHelp);
            }

            [Fact]
            public void Should_Not_Show_Version()
            {
                // Given, When
                var result = new LakeOptions();

                // Then
                Assert.False(result.ShowVersion);
            }

            [Fact]
            public void Rebuild_Should_Be_Disabled_By_Default()
            {
                // Given, When
                var result = new LakeOptions();

                // Then
                Assert.False(result.Rebuild);
            }
        }
    }
}