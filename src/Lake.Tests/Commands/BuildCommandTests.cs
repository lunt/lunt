using System;
using Lake.Commands;
using Lunt;
using Lunt.Diagnostics;
using Lunt.Runtime;
using Lunt.IO;
using Lunt.Testing;
using Moq;
using Xunit;

namespace Lake.Tests.Unit.Commands
{
    public class BuildCommandTests
    {
        public class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Build_Log_Is_Null()
            {
                // Given
                var console = new Mock<IConsoleWriter>().Object;
                var scannerFactory = new Mock<IPipelineScannerFactory>().Object;
                var environment = new Mock<IBuildEnvironment>().Object;

                // When
                var result = Record.Exception(() => new BuildCommand(null, console, scannerFactory, environment));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("log", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Console_Is_Null()
            {
                // Given
                var log = new Mock<IBuildLog>().Object;
                var scannerFactory = new Mock<IPipelineScannerFactory>().Object;
                var environment = new Mock<IBuildEnvironment>().Object;

                // When
                var result = Record.Exception(() => new BuildCommand(log, null, scannerFactory, environment));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("console", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Scanner_Is_Null()
            {
                // Given
                var log = new Mock<IBuildLog>().Object;
                var console = new Mock<IConsoleWriter>().Object;
                var environment = new Mock<IBuildEnvironment>().Object;

                // When
                var result = Record.Exception(() => new BuildCommand(log, console, null, environment));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("scannerFactory", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var log = new Mock<IBuildLog>().Object;
                var console = new Mock<IConsoleWriter>().Object;
                var scannerFactory = new Mock<IPipelineScannerFactory>().Object;

                // When
                var result = Record.Exception(() => new BuildCommand(log, console, scannerFactory, null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("environment", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_File_System_From_Environment_Is_Null()
            {
                // Given
                var log = new Mock<IBuildLog>().Object;
                var console = new Mock<IConsoleWriter>().Object;
                var scannerFactory = new Mock<IPipelineScannerFactory>().Object;
                var environment = new Mock<IBuildEnvironment>().Object;

                // When
                var result = Record.Exception(() => new BuildCommand(log, console, scannerFactory, environment));

                // Then
                Assert.IsType<ArgumentException>(result);
                Assert.True(result.Message.StartsWith("The build environment's file system was null."));
            }
        }

        public class TheExecuteMethod
        {
            [Fact]
            public void Should_Throw_If_Configuration_Is_Null()
            {
                // Given
                var factory = new BuildCommandFactory();
                factory.Options.BuildConfiguration = null;

                var command = factory.Create();

                // When
                var result = Record.Exception(() => command.Execute(factory.Options));

                // Then
                Assert.IsType<LuntException>(result);
                Assert.Equal("Build configuration file path has not been set.", result.Message);
            }

            [Fact]
            public void Should_Make_Configuration_File_Absolute_If_Relative()
            {
                // Given
                var factory = new BuildCommandFactory(); 
                factory.Options.BuildConfiguration = "build.config";

                var command = factory.Create();

                // When
                command.Execute(factory.Options);

                // Then
                factory.ConfigurationReader.Verify(x => x.Read(
                    It.Is<FilePath>(path => path.FullPath == "/working/build.config")));
            }

            [Fact]
            public void Should_Default_Input_Directory_To_Working_Directory_If_Not_Set()
            {
                // Given
                var factory = new BuildCommandFactory();
                factory.Options.InputDirectory = null;

                var command = factory.Create();

                // When
                command.Execute(factory.Options);

                // Then
                factory.Engine.Verify(x => x.Build(
                    It.Is<BuildConfiguration>(configuration => configuration.InputDirectory.FullPath == "/working"),
                    It.IsAny<BuildManifest>()));
            }

            [Fact]
            public void Should_Default_Output_Directory_To_Working_Directory_If_Not_Set()
            {
                // Given
                var factory = new BuildCommandFactory();
                factory.Options.OutputDirectory = null;

                var command = factory.Create();

                // When
                command.Execute(factory.Options);

                // Then
                factory.Engine.Verify(x => x.Build(
                    It.Is<BuildConfiguration>(configuration => configuration.OutputDirectory.FullPath == "/working"),
                    It.IsAny<BuildManifest>()));
            }

            [Fact]
            public void Should_Make_Input_Directory_Absolute_If_Relative()
            {
                // Given
                var factory = new BuildCommandFactory();
                factory.Options.InputDirectory = "input";

                var command = factory.Create();

                // When
                command.Execute(factory.Options);

                // Then
                factory.Engine.Verify(x => x.Build(
                    It.Is<BuildConfiguration>(configuration => configuration.InputDirectory.FullPath == "/working/input"),
                    It.IsAny<BuildManifest>()));
            }

            [Fact]
            public void Should_Make_Output_Directory_Absolute_If_Relative()
            {
                // Given
                var factory = new BuildCommandFactory();
                factory.Options.OutputDirectory = "output";

                var command = factory.Create();

                // When
                command.Execute(factory.Options);

                // Then
                factory.Engine.Verify(x => x.Build(
                    It.Is<BuildConfiguration>(configuration => configuration.OutputDirectory.FullPath == "/working/output"),
                    It.IsAny<BuildManifest>()));
            }

            [Fact]
            public void Should_Read_Build_Configuration()
            {
                // Given
                var factory = new BuildCommandFactory();

                var reader = new Mock<IBuildConfigurationReader>();
                reader.Setup(x => x.Read(It.IsAny<FilePath>()))
                    .Returns(factory.Configuration)
                    .Verifiable();

                var command = factory.Create(reader.Object);

                // When
                command.Execute(factory.Options);

                // Then
                reader.Verify();
            }

            [Fact]
            public void Should_Load_Previous_Manifest()
            {
                // Given
                var factory = new BuildCommandFactory();

                var manifestProvider = new Mock<IBuildManifestProvider>();
                manifestProvider.Setup(x => x.LoadManifest(It.IsAny<IFileSystem>(), It.IsAny<FilePath>()))
                    .Returns(() => null)
                    .Verifiable();

                var command = factory.Create(manifestProvider: manifestProvider.Object);

                // When
                command.Execute(factory.Options);

                // Then
                manifestProvider.Verify();
            }

            [Fact]
            public void Should_Build_Assets_With_Provided_Configuration_And_Manifest()
            {
                // Given
                var factory = new BuildCommandFactory();

                var previousManifest = new BuildManifest();
                var manifestProvider = new Mock<IBuildManifestProvider>();
                manifestProvider.Setup(x => x.LoadManifest(It.IsAny<IFileSystem>(), It.IsAny<FilePath>()))
                    .Returns(previousManifest);

                var engine = new Mock<IBuildEngine>();
                engine.Setup(x => x.Build(factory.Configuration, previousManifest))
                    .Returns(new BuildManifest())
                    .Verifiable();

                var command = factory.Create(manifestProvider: manifestProvider.Object, engine: engine.Object);

                // When
                command.Execute(factory.Options);

                // Then
                engine.Verify();
            }

            [Fact]
            public void Should_Save_New_Manifest()
            {
                // Given
                var factory = new BuildCommandFactory();

                var manifest = new BuildManifest();
                var manifestProvider = new Mock<IBuildManifestProvider>();
                manifestProvider.Setup(x => x.SaveManifest(It.IsAny<IFileSystem>(), It.IsAny<FilePath>(), manifest))
                    .Verifiable();

                var engine = new Mock<IBuildEngine>();
                engine.Setup(x => x.Build(It.IsAny<BuildConfiguration>(), It.IsAny<BuildManifest>()))
                    .Returns(() => manifest);

                var command = factory.Create(manifestProvider: manifestProvider.Object, engine: engine.Object);

                // When
                command.Execute(factory.Options);

                // Then
                manifestProvider.Verify();
            }

            [Fact]
            public void Will_Output_Results_To_Console()
            {
                // Given
                var factory = new BuildCommandFactory();

                factory.Manifest.Items.Add(new BuildManifestItem(new Asset("/assets/1.txt")) { Status = AssetBuildStatus.Skipped });
                factory.Manifest.Items.Add(new BuildManifestItem(new Asset("/assets/2.txt")) { Status = AssetBuildStatus.Skipped });
                factory.Manifest.Items.Add(new BuildManifestItem(new Asset("/assets/3.txt")) { Status = AssetBuildStatus.Skipped });
                factory.Manifest.Items.Add(new BuildManifestItem(new Asset("/assets/4.txt")) { Status = AssetBuildStatus.Success });
                factory.Manifest.Items.Add(new BuildManifestItem(new Asset("/assets/5.txt")) { Status = AssetBuildStatus.Success });
                factory.Manifest.Items.Add(new BuildManifestItem(new Asset("/assets/6.txt")) { Status = AssetBuildStatus.Failure });

                var command = factory.Create();

                // When    
                command.Execute(factory.Options);

                // Then
                const string expected = "\n========== Build: 2 succeeded, 1 failed, 3 skipped ==========";
                Assert.True(factory.Console.Content.Contains(expected));
            }

            [Fact]
            public void Should_Make_Probing_Directory_Absolute_If_Relative()
            {
                // Given
                var factory = new BuildCommandFactory();
                factory.Options.ProbingDirectory = "probing";
                var command = factory.Create();

                // When
                command.Execute(factory.Options);

                // Then
                factory.ScannerFactory.Verify(x => x.Create(It.Is<DirectoryPath>(path => path.FullPath == "/working/probing")));
            }

            [Fact]
            public void Will_Use_Probing_Directory_For_Scanner_Factory_If_Set()
            {
                // Given
                var factory = new BuildCommandFactory();
                factory.Options.ProbingDirectory = "/probing";
                var command = factory.Create();

                // When
                command.Execute(factory.Options);

                // Then
                factory.ScannerFactory.Verify(x => x.Create(It.Is<DirectoryPath>(path => path.FullPath=="/probing")));
            }

            [Fact]
            public void Will_Use_Working_Directory_For_Scanner_Factory_If_Probing_Directory_Has_Not_Been_Set()
            {
                // Given
                var factory = new BuildCommandFactory();
                var command = factory.Create();

                // When
                command.Execute(factory.Options);

                // Then
                factory.ScannerFactory.Verify(x => x.Create(It.Is<DirectoryPath>(path => path.FullPath == "/working")));
            }
        }
    }
}