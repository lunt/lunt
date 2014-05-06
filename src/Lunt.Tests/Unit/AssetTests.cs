using System;
using Lunt.IO;
using Xunit;

namespace Lunt.Tests.Unit
{
    public class AssetTests
    {
        public class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Asset_Path_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new Asset(null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("path", ((ArgumentNullException) result).ParamName);
            }

            [Fact]
            public void Should_Have_Non_Null_Metadata_Reference_When_No_Metadata_Was_Passed()
            {
                // Given, When
                var result = new Asset("input/simple.asset");

                // Then
                Assert.NotNull(result.Metadata);
            }
        }

        public class GetSourcePathMethod
        {
            [Fact]
            public void Should_Throw_If_Build_Configuration_Is_Null()
            {
                // Given
                var asset = new Asset("input/simple.asset");

                // When
                var result = Record.Exception(() => asset.GetSourceFilePath(null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("configuration", ((ArgumentNullException) result).ParamName);
            }

            [Fact]
            public void Can_Get_Source_File_Path()
            {
                // Given
                var configuration = new BuildConfiguration();
                configuration.InputDirectory = "assets";
                var asset = new Asset("shaders/basic.vs");

                // When
                var result = asset.GetSourceFilePath(configuration);

                // Then
                Assert.Equal("assets/shaders/basic.vs", result.FullPath);
            }
        }

        public class GetTargetPathMethod
        {
            [Fact]
            public void Should_Throw_If_Build_Configuration_Is_Null()
            {
                // Given
                var asset = new Asset("input/simple.asset");

                // When
                var result = Record.Exception(() => asset.GetTargetFilePath(null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("configuration", ((ArgumentNullException) result).ParamName);
            }

            [Fact]
            public void Can_Get_Target_File_Path()
            {
                // Given
                var configuration = new BuildConfiguration();
                configuration.OutputDirectory = "output";
                var asset = new Asset("shaders/basic.vs");

                // When
                var result = asset.GetTargetFilePath(configuration);

                // Then
                Assert.Equal("output/shaders/basic.dat", result.FullPath);
            }
        }

        [Fact]
        public void Can_Get_Asset_Path()
        {
            // Given
            var path = new FilePath("shaders/basic");

            // When
            var configuration = new Asset(path);

            // Then
            Assert.Equal(path, configuration.Path);
        }
    }
}