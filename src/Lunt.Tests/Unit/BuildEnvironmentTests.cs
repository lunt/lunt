using System;
using Xunit;

namespace Lunt.Tests.Unit
{
    public class BuildEnvironmentTests
    {
        public class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_File_System_Is_Null()
            {
                // Given
                var result = Record.Exception(() => new BuildEnvironment(null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("fileSystem", ((ArgumentNullException)result).ParamName);
            }
        }
    }
}
