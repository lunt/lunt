using System;
using Lunt.IO;
using Lunt.Testing;
using Moq;
using Xunit;

namespace Lunt.Tests.Unit
{
    public sealed class BuildEngineTests
    {
        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Options_Is_Null()
            {
                // Given, When, Then
                Assert.Throws<ArgumentNullException>(() => new BuildEngine().Run(null))
                    .ShouldHaveParameter("settings");
            }

            [Fact]
            public void Should_Make_Relative_Build_Configuration_File_Absolute_To_Working_Direcory()
            {
                // Given
                var file = new FilePath("file.xml");
                var config = new FakeInternalConfiguration(file);
                var engine = new BuildEngine(config);

                // When
                engine.Run(new BuildEngineSettings(file));

                // Then
                config.BuildConfigurationReader.Verify(x => x.Read(
                    It.Is<FilePath>(p => p.FullPath == "/working/file.xml")));
            }

            [Fact]
            public void Should_Set_Input_Directory_To_Configuration_Directory_If_Not_Specified()
            {
                // Given
                var file = new FilePath("/root/file.xml");
                var config = new FakeInternalConfiguration(file);
                var engine = new BuildEngine(config);

                // When
                engine.Run(new BuildEngineSettings(file));

                // Then
                config.BuildKernel.Verify(x => x.Build(
                    It.Is<BuildConfiguration>(c => c.InputDirectory.FullPath == "/root")));
            }

            [Fact]
            public void Should_Set_Output_Directory_To_Working_Directory_If_Not_Specified()
            {
                // Given
                var file = new FilePath("/root/file.xml");
                var config = new FakeInternalConfiguration(file);
                var engine = new BuildEngine(config);

                // When
                engine.Run(new BuildEngineSettings(file));

                // Then
                config.BuildKernel.Verify(x => x.Build(
                    It.Is<BuildConfiguration>(c => c.OutputDirectory.FullPath == "/working/Output")));
            }

            [Fact]
            public void Should_Set_Input_Directory_To_Specified_Directory()
            {
                // Given
                var file = new FilePath("/root/file.xml");
                var config = new FakeInternalConfiguration(file);
                var engine = new BuildEngine(config);

                var settings = new BuildEngineSettings(file);
                settings.InputPath = "/Input";

                // When
                engine.Run(settings);

                // Then
                config.BuildKernel.Verify(x => x.Build(
                    It.Is<BuildConfiguration>(c => c.InputDirectory.FullPath == "/Input")));
            }

            [Fact]
            public void Should_Append_Working_Directory_To_Specified_Relative_Input_Directory()
            {
                // Given
                var file = new FilePath("/root/file.xml");
                var config = new FakeInternalConfiguration(file);
                var engine = new BuildEngine(config);

                var settings = new BuildEngineSettings(file);
                settings.InputPath = "Input";

                // When
                engine.Run(settings);

                // Then
                config.BuildKernel.Verify(x => x.Build(
                    It.Is<BuildConfiguration>(c => c.InputDirectory.FullPath == "/working/Input")));
            }

            [Fact]
            public void Should_Set_Output_Directory_To_Specified_Directory()
            {
                // Given
                var file = new FilePath("file.xml");
                var config = new FakeInternalConfiguration(file);
                var engine = new BuildEngine(config);

                var settings = new BuildEngineSettings(file);
                settings.OutputPath = "/Output";

                // When
                engine.Run(settings);

                // Then
                config.BuildKernel.Verify(x => x.Build(
                    It.Is<BuildConfiguration>(c => c.OutputDirectory.FullPath == "/Output")));
            }

            [Fact]
            public void Should_Append_Working_Directory_To_Specified_Relative_Output_Directory()
            {
                // Given
                var file = new FilePath("file.xml");
                var config = new FakeInternalConfiguration(file);
                var engine = new BuildEngine(config);

                var settings = new BuildEngineSettings(file);
                settings.OutputPath = "Output";

                // When
                engine.Run(settings);

                // Then
                config.BuildKernel.Verify(x => x.Build(
                    It.Is<BuildConfiguration>(c => c.OutputDirectory.FullPath == "/working/Output")));
            }
        }

    }
}
