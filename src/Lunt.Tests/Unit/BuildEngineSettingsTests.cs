using System;
using Lunt.IO;
using Lunt.Testing;
using Xunit;

namespace Lunt.Tests.Unit
{
    public sealed class DebuggerOptionsTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_The_Build_Configuration_Path_Is_Null()
            {
                // Given, When, Then
                Assert.Throws<ArgumentNullException>(() => new BuildEngineSettings(null))
                    .ShouldHaveParameter("buildConfigurationPath");
            }
        }

        public sealed class TheBuildConfigurationPathProperty
        {
            [Fact]
            public void Should_Return_The_Provided_Build_Configuration_Path()
            {
                // Given
                var path = new FilePath("/file.xml");
                var options = new BuildEngineSettings(path);

                // When, Then
                Assert.Equal(path, options.BuildConfigurationPath);
            }
        }
    }
}
