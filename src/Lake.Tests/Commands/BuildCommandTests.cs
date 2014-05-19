using System;
using Lake.Commands;
using Lunt;
using Lunt.Diagnostics;
using Lunt.Runtime;
using Lunt.IO;
using Lunt.Testing;
using Lunt.Testing.Utilities;
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
                Assert.True(result.Message.StartsWith("The build environment's file system was null.", StringComparison.Ordinal));
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
                var command = factory.CreateCommand();

                // When, Then
                Assert.Throws<LuntException>(() => command.Execute(factory.Options))
                    .ShouldHaveMessage("Build configuration file path has not been set.");
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

                // When
                factory.CreateCommand().Execute(factory.Options);

                // Then
                const string expected = "\n========== Build: 2 succeeded, 1 failed, 3 skipped ==========";
                Assert.True(factory.Console.Content.Contains(expected));
            }

            [Fact]
            public void Will_Use_Probing_Directory_For_Scanner_Factory_If_Set()
            {
                // Given
                var factory = new BuildCommandFactory();
                factory.Options.ProbingDirectory = "/Assemblies";

                // When
                factory.CreateCommand().Execute(factory.Options);

                // Then
                factory.ScannerFactory.Verify(x => x.Create(
                    It.Is<DirectoryPath>(p => p.FullPath == "/Assemblies")));
            }

            [Fact]
            public void Should_Make_Probing_Directory_Absolute_If_Relative()
            {
                // Given
                var factory = new BuildCommandFactory();
                factory.Options.ProbingDirectory = "Relative";

                // When
                factory.CreateCommand().Execute(factory.Options);

                // Then
                factory.ScannerFactory.Verify(x => x.Create(
                    It.Is<DirectoryPath>(p => p.FullPath == "/Working/Relative")));
            }

            [Fact]
            public void Will_Use_Working_Directory_For_Scanner_Factory_If_Probing_Directory_Has_Not_Been_Set()
            {
                // Given
                var factory = new BuildCommandFactory();
                factory.Options.ProbingDirectory = null;

                // When
                factory.CreateCommand().Execute(factory.Options);

                // Then
                factory.ScannerFactory.Verify(x => x.Create(
                    It.Is<DirectoryPath>(p => p.FullPath == "/Working")));
            }

            [Fact]
            public void Should_Return_Success_If_Build_Was_Successful()
            {
                // Given
                var factory = new BuildCommandFactory();
                factory.Manifest.Items.Add(new BuildManifestItem(new Asset("/assets/4.txt")) { Status = AssetBuildStatus.Success });

                // When
                var result = factory.CreateCommand().Execute(factory.Options);

                // Then
                Assert.Equal((int)ExitCode.Success, result);
            }

            [Fact]
            public void Should_Return_Success_Even_If_Assets_Were_Skipped()
            {
                // Given
                var factory = new BuildCommandFactory();
                factory.Manifest.Items.Add(new BuildManifestItem(new Asset("/assets/3.txt")) { Status = AssetBuildStatus.Skipped });
                factory.Manifest.Items.Add(new BuildManifestItem(new Asset("/assets/4.txt")) { Status = AssetBuildStatus.Success });
                factory.Manifest.Items.Add(new BuildManifestItem(new Asset("/assets/5.txt")) { Status = AssetBuildStatus.Success });

                // When
                var result = factory.CreateCommand().Execute(factory.Options);

                // Then
                Assert.Equal((int)ExitCode.Success, result);
            }

            [Fact]
            public void Should_Return_Failure_If_Build_Of_Any_Asset_Failed()
            {
                // Given
                var factory = new BuildCommandFactory();
                factory.Manifest.Items.Add(new BuildManifestItem(new Asset("/assets/1.txt")) { Status = AssetBuildStatus.Skipped });
                factory.Manifest.Items.Add(new BuildManifestItem(new Asset("/assets/2.txt")) { Status = AssetBuildStatus.Skipped });
                factory.Manifest.Items.Add(new BuildManifestItem(new Asset("/assets/3.txt")) { Status = AssetBuildStatus.Skipped });
                factory.Manifest.Items.Add(new BuildManifestItem(new Asset("/assets/4.txt")) { Status = AssetBuildStatus.Success });
                factory.Manifest.Items.Add(new BuildManifestItem(new Asset("/assets/5.txt")) { Status = AssetBuildStatus.Success });
                factory.Manifest.Items.Add(new BuildManifestItem(new Asset("/assets/6.txt")) { Status = AssetBuildStatus.Failure });

                // When
                var result = factory.CreateCommand().Execute(factory.Options);

                // Then
                Assert.Equal((int)ExitCode.BuildFailure, result);
            }
        }
    }
}