using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lunt.IO;
using Xunit;

namespace Lunt.Tests.Unit.Core.IO
{
    public class PathNormalizerTests
    {
        [Fact]
        public void Should_Throw_If_Path_Is_Null()
        {
            // Given, When
            var result = Record.Exception(() => PathNormalizer.Collapse(null));

            // Then
            Assert.IsType<ArgumentNullException>(result);
            Assert.Equal("path", ((ArgumentNullException)result).ParamName);
        }

        [Fact]
        public void Should_Throw_If_Path_Is_Relative()
        {
            // Given, When
            var result = Record.Exception(() => PathNormalizer.Collapse(new DirectoryPath("hello/temp/test/../../world")));

            // Then
            Assert.IsType<ArgumentException>(result);
            Assert.True(result.Message.StartsWith("Path to be collapsed cannot be relative."));
            Assert.Equal("path", ((ArgumentException)result).ParamName);
        }

#if !UNIX
        [Fact]
        public void Should_Normalize_Path_With_Windows_Root()
        {
            // Given, When
            var path = PathNormalizer.Collapse(new DirectoryPath("c:/hello/temp/test/../../world"));

            // Then
            Assert.Equal("c:/hello/world", path);
        }
#endif

        [Fact]
        public void Should_Normalize_Path_With_Non_Windows_Root()
        {
            // Given, When
            var path = PathNormalizer.Collapse(new DirectoryPath("/hello/temp/test/../../world"));

            // Then
            Assert.Equal("/hello/world", path);
        }

#if !UNIX
        [Fact]
        public void Should_Stop_Normalizing_When_Windows_Root_Is_Reached()
        {
            // Given, When
            var path = PathNormalizer.Collapse(new DirectoryPath("c:/../../../../../../temp"));

            // Then
            Assert.Equal("c:/temp", path);
        }
#endif

        [Fact]
        public void Should_Stop_Normalizing_When_Root_Is_Reached()
        {
            // Given, When
            var path = PathNormalizer.Collapse(new DirectoryPath("/hello/../../../../../../temp"));

            // Then
            Assert.Equal("/temp", path);
        }
    }
}
