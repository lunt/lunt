using System;
using Lunt.IO;
using Lunt.Testing;
using Lunt.Tests.Fixtures;
using Moq;
using Xunit;

namespace Lunt.Tests.Unit.IO
{
    public class GlobberTests
    {
        public class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Environment_Provider_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new Globber(null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("environment", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_File_System_From_Environment_Is_Null()
            {
                // Given
                var environment = new Mock<IBuildEnvironment>().Object;

                // When
                var result = Record.Exception(() => new Globber(environment));

                // Then
                Assert.IsType<ArgumentException>(result);
                Assert.True(result.Message.StartsWith("The build environment's file system was null."));
            }
        }

        public class TheGlobMethod : IUseFixture<GlobberFixture>
        {
            private GlobberFixture _fixture;

            public void SetFixture(GlobberFixture data)
            {
                _fixture = data;
            }

            [Fact]
            public void Should_Throw_If_Pattern_Is_Empty()
            {
                // Given
                var environment = new FakeBuildEnvironment(_fixture.FileSystem);
                var globber = new Globber(environment);

                // When
                var result = Record.Exception(() => globber.Glob(null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("pattern", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Can_Traverse_Recursivly()
            {
                // Given
                var environment = new FakeBuildEnvironment(_fixture.FileSystem);
                var globber = new Globber(environment);

                // When
                var result = globber.Glob("/Temp/**/*.txt");

                // Then
                Assert.Equal(2, result.Length);
                Assert.Equal("/Temp/Hello/World/Text.txt", result[0].FullPath);
                Assert.Equal("/Temp/Goodbye/OtherText.txt", result[1].FullPath);
            }

#if !UNIX
            [Fact]
            public void Will_Fix_Root_If_Drive_Is_Missing_By_Using_The_Drive_From_The_Working_Directory()
            {
                // Given
                var environment = new FakeBuildEnvironment(_fixture.FileSystem, "C:/Working/");
                var globber = new Globber(environment);

                // When
                var result = globber.Glob("/Temp/Hello/World/Text.txt");

                // Then
                Assert.Equal(1, result.Length);
                Assert.Equal("C:/Temp/Hello/World/Text.txt", result[0].FullPath);
            }
#endif

            [Fact]
            public void Will_Append_Relative_Root_With_Implicit_Working_Directory()
            {
                // Given
                var environment = new FakeBuildEnvironment(_fixture.FileSystem);
                var globber = new Globber(environment);

                // When
                var result = globber.Glob("Hello/World/Text.txt");

                // Then
                Assert.Equal(1, result.Length);
                Assert.Equal("/Working/Hello/World/Text.txt", result[0].FullPath);
            }

            [Fact]
            public void Should_Throw_If_Unc_Root_Was_Encountered()
            {
                // Given
                var environment = new FakeBuildEnvironment(_fixture.FileSystem);
                var globber = new Globber(environment);

                // When
                var result = Record.Exception(() => globber.Glob("//Hello/World/Text.txt"));

                // Then
                Assert.IsType<LuntException>(result);
                Assert.Equal("UNC paths are not supported.", result.Message);
            }
        }
    }
}
