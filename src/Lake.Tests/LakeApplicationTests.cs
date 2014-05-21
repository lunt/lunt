using System;
using Lake.Arguments;
using Lake.Commands;
using Lake.Diagnostics;
using Lunt;
using Lunt.Testing;
using NSubstitute;
using Xunit;
using Xunit.Extensions;

namespace Lake.Tests.Unit
{
    public class LakeApplicationTests
    {
        public class TheRunCommand
        {
            private LakeApplication CreateApplication(ICommandFactory factory, IArgumentParser parser = null)
            {
                var console = Substitute.For<IConsoleWriter>();
                var log = Substitute.For<ILakeBuildLog>();                
                parser = parser ?? new ArgumentParser(log);
                return new LakeApplication(console, log, parser, factory);
            }

            [Theory]
            [InlineData("-help")]
            [InlineData("-?")]
            [InlineData("-h")]
            public void Should_Show_Usage_Information(string command)
            {
                // Given
                var factory = Substitute.For<ICommandFactory>();
                factory.CreateHelpCommand(Arg.Any<LakeOptions>()).Returns(r => new FakeCommand());

                // When
                CreateApplication(factory).Run(new[] { command });

                // Then
                factory.Received(1).CreateHelpCommand(Arg.Any<LakeOptions>());
            }

            [Theory]
            [InlineData("-version")]
            [InlineData("-ver")]
            public void Should_Show_Version_Information(string command)
            {
                // Given
                var factory = Substitute.For<ICommandFactory>();
                factory.CreateVersionCommand(Arg.Any<LakeOptions>()).Returns(r => new FakeCommand());

                // When
                CreateApplication(factory).Run(new[] {command});

                // Then
                factory.Received(1).CreateVersionCommand(Arg.Any<LakeOptions>());
            }

            [Fact]
            public void Should_Build_If_Output_Directory_And_Build_Configuration_Is_Set()
            {
                // Given
                var factory = Substitute.For<ICommandFactory>();
                factory.CreateBuildCommand(Arg.Any<LakeOptions>()).Returns(r => new FakeCommand());

                // When
                CreateApplication(factory).Run(new[] {"-input='/assets'", "-output='/output'", "build.config"});

                // Then
                factory.Received(1).CreateBuildCommand(Arg.Any<LakeOptions>());
            }

            [Fact]
            public void Should_Show_Usage_Information_If_Options_Are_Null()
            {
                // Given
                var factory = Substitute.For<ICommandFactory>();
                factory.CreateHelpCommand(Arg.Any<LakeOptions>()).Returns(r => new FakeCommand());

                var parser = Substitute.For<IArgumentParser>();
                parser.Parse(Arg.Any<string[]>()).Returns((LakeOptions)null);

                // When
                CreateApplication(factory, parser).Run(new string[] { });

                // Then
                factory.Received(1).CreateHelpCommand(Arg.Any<LakeOptions>());
            }

            [Fact]
            public void Should_Show_Usage_Information_If_No_Options_Are_Set()
            {
                // Given
                var factory = Substitute.For<ICommandFactory>();
                factory.CreateHelpCommand(Arg.Any<LakeOptions>()).Returns(r => new FakeCommand());

                var parser = Substitute.For<IArgumentParser>();
                parser.Parse(Arg.Any<string[]>()).Returns(new LakeOptions());

                // When
                CreateApplication(factory, parser).Run(new string[] { });

                // Then
                factory.Received(1).CreateHelpCommand(Arg.Any<LakeOptions>());
            }

            [Fact]
            public void Should_Catch_Exceptions_In_Commands()
            {
                // Given
                var factory = Substitute.For<ICommandFactory>();
                factory.CreateVersionCommand(Arg.Any<LakeOptions>()).Returns(r => new FakeCommand(() => { throw new InvalidOperationException(); }));

                // When
                var result = CreateApplication(factory).Run(new string[] { "-version" });

                // Then
                factory.Received(1).CreateVersionCommand(Arg.Any<LakeOptions>());
                Assert.Equal((int)ExitCode.Exception, result);
            }
        }
    }
}