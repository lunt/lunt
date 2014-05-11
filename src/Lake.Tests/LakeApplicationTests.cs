using System;
using Lake.Arguments;
using Lake.Commands;
using Lake.Diagnostics;
using Lunt;
using Lunt.Testing;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Lake.Tests.Unit
{
    public class LakeApplicationTests
    {
        public class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Console_Is_Null()
            {
                
            }
        }

        public class TheRunCommand
        {
            [Theory]
            [InlineData("-help")]
            [InlineData("-?")]
            [InlineData("-h")]
            public void Should_Show_Usage_Information(string command)
            {
                // Given
                var log = new Mock<ILakeBuildLog>().Object;
                var console = new Mock<IConsoleWriter>().Object;
                var parser = new ArgumentParser(log);
                var factory = new Mock<ICommandFactory>();
                factory.Setup(x => x.CreateHelpCommand(It.IsAny<LakeOptions>()))
                    .Returns(() => new FakeCommand()).Verifiable();

                var application = new LakeApplication(console, log, parser, factory.Object);

                // When
                application.Run(new[] {command});

                // Then
                factory.Verify();
            }

            [Theory]
            [InlineData("-version")]
            [InlineData("-ver")]
            public void Should_Show_Version_Information(string command)
            {
                // Given
                var log = new Mock<ILakeBuildLog>().Object;
                var console = new Mock<IConsoleWriter>().Object;
                var parser = new ArgumentParser(log);
                var factory = new Mock<ICommandFactory>();
                factory.Setup(x => x.CreateVersionCommand(It.IsAny<LakeOptions>()))
                    .Returns(() => new FakeCommand()).Verifiable();

                var application = new LakeApplication(console, log, parser, factory.Object);

                // When
                application.Run(new[] {command});

                // Then
                factory.Verify();
            }

            [Fact]
            public void Should_Build_If_Output_Directory_And_Build_Configuration_Is_Set()
            {
                // Given
                var log = new Mock<ILakeBuildLog>().Object;
                var console = new Mock<IConsoleWriter>().Object;
                var parser = new ArgumentParser(log);

                var factory = new Mock<ICommandFactory>();
                factory.Setup(x => x.CreateBuildCommand(It.IsAny<LakeOptions>()))
                    .Returns(() => new FakeCommand()).Verifiable();

                // When
                new LakeApplication(console, log, parser, factory.Object)
                    .Run(new[] {"-input='/assets'", "-output='/output'", "build.config"});

                // Then
                factory.Verify();
            }

            [Fact]
            public void Should_Show_Usage_Information_If_Options_Are_Null()
            {
                // Given
                var log = new Mock<ILakeBuildLog>().Object;
                var console = new Mock<IConsoleWriter>().Object;
                var parser = new Mock<IArgumentParser>();
                parser.Setup(x => x.Parse(It.IsAny<string[]>()))
                    .Returns(() => null);

                var factory = new Mock<ICommandFactory>();
                factory.Setup(x => x.CreateHelpCommand(It.IsAny<LakeOptions>()))
                    .Returns(() => new FakeCommand()).Verifiable();

                // When
                new LakeApplication(console, log, parser.Object, factory.Object)
                    .Run(new string[] {});

                // Then
                factory.Verify();
            }

            [Fact]
            public void Should_Show_Usage_Information_If_No_Options_Are_Set()
            {
                // Given
                var log = new Mock<ILakeBuildLog>().Object;
                var console = new Mock<IConsoleWriter>().Object;
                var parser = new Mock<IArgumentParser>();
                parser.Setup(x => x.Parse(It.IsAny<string[]>()))
                    .Returns(() => new LakeOptions());

                var factory = new Mock<ICommandFactory>();
                factory.Setup(x => x.CreateHelpCommand(It.IsAny<LakeOptions>()))
                    .Returns(() => new FakeCommand()).Verifiable();

                // When
                new LakeApplication(console, log, parser.Object, factory.Object)
                    .Run(new string[] {});

                // Then
                factory.Verify();
            }

            [Fact]
            public void Should_Catch_Exceptions_In_Commands()
            {
                // Given
                var log = new Mock<ILakeBuildLog>().Object;
                var console = new Mock<IConsoleWriter>().Object;
                var parser = new ArgumentParser(log);
                var factory = new Mock<ICommandFactory>();
                factory.Setup(x => x.CreateVersionCommand(It.IsAny<LakeOptions>()))
                    .Returns(() => new FakeCommand(() => { throw new InvalidOperationException(); }))
                    .Verifiable();

                // When
                var result = new LakeApplication(console, log, parser, factory.Object)
                    .Run(new[] {"-version"});

                // Then
                Assert.Equal(1, result);
                factory.Verify();
            }
        }
    }
}