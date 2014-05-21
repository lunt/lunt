using System;
using Lake.Commands;
using Lunt;
using Lunt.Diagnostics;
using Lunt.Runtime;
using NSubstitute;
using Xunit;

namespace Lake.Tests.Unit.Commands
{
    public sealed class CommandFactoryTests
    {
        internal static CommandFactory CreateFactory()
        {
            var log = Substitute.For<IBuildLog>();
            var console = Substitute.For<IConsoleWriter>();
            var environment = Substitute.For<IBuildEnvironment>();
            var scannerFactory = Substitute.For<IPipelineScannerFactory>();
            return new CommandFactory(log, console, environment, scannerFactory);
        }

        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Build_Log_Is_Null()
            {
                // Given
                var console = Substitute.For<IConsoleWriter>();
                var environment = Substitute.For<IBuildEnvironment>();
                var factory = Substitute.For<IPipelineScannerFactory>();

                // When
                var result = Record.Exception(() => new CommandFactory(null, console, environment, factory));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("log", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Console_Is_Null()
            {
                // Given
                var log = Substitute.For<IBuildLog>();
                var environment = Substitute.For<IBuildEnvironment>();
                var factory = Substitute.For<IPipelineScannerFactory>();

                // When
                var result = Record.Exception(() => new CommandFactory(log, null, environment, factory));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("console", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var log = Substitute.For<IBuildLog>();
                var console = Substitute.For<IConsoleWriter>();
                var factory = Substitute.For<IPipelineScannerFactory>();

                // When
                var result = Record.Exception(() => new CommandFactory(log, console, null, factory));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("environment", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Scanner_Factory_Is_Null()
            {
                // Given
                var log = Substitute.For<IBuildLog>();
                var console = Substitute.For<IConsoleWriter>();
                var environment = Substitute.For<IBuildEnvironment>();

                // When
                var result = Record.Exception(() => new CommandFactory(log, console, environment, null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("factory", ((ArgumentNullException)result).ParamName);
            }
        }

        public sealed class TheCreateHelpCommandMethod
        {
            [Fact]
            public void Will_Build_Help_Command()
            {
                // Given
                var factory = CommandFactoryTests.CreateFactory();

                // When
                var result = factory.CreateHelpCommand(new LakeOptions());

                // Then
                Assert.IsType<ShowHelpCommand>(result);
            }
        }

        public sealed class TheCreateVersionCommandMethod
        {
            [Fact]
            public void Will_Build_Help_Command()
            {
                // Given
                var factory = CommandFactoryTests.CreateFactory();

                // When
                var result = factory.CreateVersionCommand(new LakeOptions());

                // Then
                Assert.IsType<ShowVersionCommand>(result);
            }
        }

        public sealed class TheCreateBuildCommandMethod
        {
            [Fact]
            public void Will_Build_Help_Command()
            {
                // Given
                var factory = CommandFactoryTests.CreateFactory();

                // When
                var result = factory.CreateBuildCommand(new LakeOptions());

                // Then
                Assert.IsType<BuildCommand>(result);
            }
        }
    }
}