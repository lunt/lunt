using System;
using Lunt.IO;
using Xunit;
using Xunit.Extensions;

namespace Lunt.Tests.Unit.IO
{
    public class PathTests
    {
        #region Private Test Classes

        private class ConcretePath : Path
        {
            public ConcretePath(string path)
                : base(path)
            {
            }
        }

        #endregion

        public class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new ConcretePath(null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("path", ((ArgumentNullException) result).ParamName);
            }

            [Theory]
            [InlineData("")]
            [InlineData("\t ")]
            public void Should_Throw_If_Path_Is_Empty(string fullPath)
            {
                // Given, When
                var result = Record.Exception(() => new ConcretePath(fullPath));

                // Then
                Assert.IsType<ArgumentException>(result);
                Assert.Equal("path", ((ArgumentException) result).ParamName);
                Assert.Equal(string.Format("Path cannot be empty.{0}Parameter name: path", Environment.NewLine), result.Message);
            }

            [Fact]
            public void Current_Directory_Returns_Empty_Path()
            {
                // Given, When
                var path = new ConcretePath("./");

                // Then
                Assert.Equal(string.Empty, path.FullPath);
            }

            [Fact]
            public void Will_Normalize_Path_Separators()
            {
                // Given, When
                var path = new ConcretePath("shaders\\basic");

                // Then
                Assert.Equal("shaders/basic", path.FullPath);
            }

            [Fact]
            public void Will_Trim_WhiteSpace_From_Path()
            {
                // Given, When
                var path = new ConcretePath(" shaders/basic ");

                // Then
                Assert.Equal("shaders/basic", path.FullPath);
            }

            [Fact]
            public void Will_Not_Remove_WhiteSpace_Within_Path()
            {
                // Given, When
                var path = new ConcretePath("my awesome shaders/basic");

                // Then
                Assert.Equal("my awesome shaders/basic", path.FullPath);
            }
        }

        [Fact]
        public void Can_Access_Full_Path()
        {
            // Given, When
            const string expected = "shaders/basic";
            var path = new ConcretePath(expected);

            // Then
            Assert.Equal(expected, path.FullPath);
        }

        [Theory]
#if !UNIX
        [InlineData("c:/assets/shaders", false)]
        [InlineData("c:/assets/shaders/basic.frag", false)]
        [InlineData("c:/", false)]
        [InlineData("c:", false)]
#endif
        [InlineData("assets/shaders", true)]
        [InlineData("assets/shaders/basic.frag", true)]
        [InlineData("/assets/shaders", false)]
        [InlineData("/assets/shaders/basic.frag", false)]
        public void Can_See_If_A_Path_Is_Relative(string fullPath, bool expected)
        {
            // Given, When
            var path = new ConcretePath(fullPath);

            // Then
            Assert.Equal(expected, path.IsRelative);
        }

        [Fact]
        public void ToString_Returns_The_Full_Path()
        {
            // Given, When
            var path = new ConcretePath("temp/hello");

            // Then
            Assert.Equal("temp/hello", path.ToString());
        }
    }
}