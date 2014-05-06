using System;
using Lake.Commands;
using Lunt;
using Lunt.Diagnostics;
using Lunt.IO;
using Lunt.Runtime;
using Lunt.Testing;
using Moq;
using Xunit;

namespace Lake.Tests.Unit.Commands
{
    public class CommandFactoryTests
    {
        public class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Build_Log_Is_Null()
            {
                // Given
                var console = new Mock<IConsoleWriter>().Object;
                var environment = new Mock<IBuildEnvironment>().Object;
                var scannerFactory = new Mock<IPipelineScannerFactory>().Object;

                // When
                var result = Record.Exception(() => new CommandFactory(null, console, environment, scannerFactory));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("log", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Console_Is_Null()
            {
                // Given
                var log = new Mock<IBuildLog>().Object;
                var environment = new Mock<IBuildEnvironment>().Object;
                var scannerFactory = new Mock<IPipelineScannerFactory>().Object;

                // When
                var result = Record.Exception(() => new CommandFactory(log, null, environment, scannerFactory));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("console", ((ArgumentNullException)result).ParamName);
            }
        }

        public class TheCreateHelpCommandMethod
        {
            [Fact]
            public void Will_Build_Help_Command()
            {
                // Given
                var log = new Mock<IBuildLog>().Object;
                var console = new Mock<IConsoleWriter>().Object;
                var environment = new Mock<IBuildEnvironment>().Object;
                var scannerFactory = new Mock<IPipelineScannerFactory>().Object;

                var factory = new CommandFactory(log, console, environment, scannerFactory);

                // When
                var result = factory.CreateHelpCommand(new LakeOptions());

                // Then
                Assert.IsType<ShowHelpCommand>(result);
            }
        }

        public class TheCreateVersionCommandMethod
        {
            [Fact]
            public void Will_Build_Help_Command()
            {
                // Given
                var log = new Mock<IBuildLog>().Object;
                var console = new Mock<IConsoleWriter>().Object;
                var environment = new Mock<IBuildEnvironment>().Object;
                var scannerFactory = new Mock<IPipelineScannerFactory>().Object;

                var factory = new CommandFactory(log, console, environment, scannerFactory);

                // When
                var result = factory.CreateVersionCommand(new LakeOptions());

                // Then
                Assert.IsType<ShowVersionCommand>(result);
            }
        }

        public class TheCreateBuildCommandMethod
        {
            [Fact]
            public void Will_Build_Help_Command()
            {
                // Given
                var log = new Mock<IBuildLog>().Object;
                var console = new Mock<IConsoleWriter>().Object;
                var environment = new Mock<IBuildEnvironment>();
                var scannerFactory = new Mock<IPipelineScannerFactory>();

                environment.Setup(x => x.GetWorkingDirectory()).Returns(new DirectoryPath("/temp"));
                environment.Setup(x => x.FileSystem).Returns(new FakeFileSystem());
                scannerFactory.Setup(x => x.Create(It.IsAny<DirectoryPath>())).Returns(new Mock<IPipelineScanner>().Object);

                var factory = new CommandFactory(log, console, environment.Object, scannerFactory.Object);

                // When
                var result = factory.CreateBuildCommand(new LakeOptions());

                // Then
                Assert.IsType<BuildCommand>(result);
            }
        }
    }
}