// ﻿
// Copyright (c) 2013 Patrik Svensson
// 
// This file is part of Lunt.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
using System;
using Lunt.Arguments;
using Lunt.Commands;
using Lunt.Diagnostics;
using Lunt.Tests.Fakes;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Lunt.Tests.Unit
{
    public class LuntApplicationTests
    {
        [Theory]
        [InlineData("-help")]
        [InlineData("-?")]
        [InlineData("-h")]
        public void Should_Show_Usage_Information(string command)
        {
            // Given
            var log = new Mock<IConsoleBuildLog>().Object;
            var console = new Mock<IConsoleWriter>().Object;
            var parser = new ArgumentParser(log);
            var factory = new Mock<ICommandFactory>();
            factory.Setup(x => x.CreateHelpCommand(It.IsAny<LuntOptions>()))
                .Returns(() => new FakeCommand()).Verifiable();

            var application = new LuntApplication(console, log, parser, factory.Object);

            // When
            application.Run(new[] { command });

            // Then
            factory.Verify();
        }

        [Theory]
        [InlineData("-version")]
        [InlineData("-ver")]
        public void Should_Show_Version_Information(string command)
        {
            // Given
            var log = new Mock<IConsoleBuildLog>().Object;
            var console = new Mock<IConsoleWriter>().Object;
            var parser = new ArgumentParser(log);
            var factory = new Mock<ICommandFactory>();
            factory.Setup(x => x.CreateVersionCommand(It.IsAny<LuntOptions>()))
                .Returns(() => new FakeCommand()).Verifiable();

            var application = new LuntApplication(console, log, parser, factory.Object);

            // When
            application.Run(new[] { command });

            // Then
            factory.Verify();
        }

        [Fact]
        public void Should_Build_If_Output_Directory_And_Build_Configuration_Is_Set()
        {
            // Given
            var log = new Mock<IConsoleBuildLog>().Object;
            var console = new Mock<IConsoleWriter>().Object;
            var parser = new ArgumentParser(log);

            var factory = new Mock<ICommandFactory>();
            factory.Setup(x => x.CreateBuildCommand(It.IsAny<LuntOptions>()))
                .Returns(() => new FakeCommand()).Verifiable();

            // When
            new LuntApplication(console, log, parser, factory.Object)
                .Run(new[] { "-input='/assets'", "-output='/output'", "build.config" });

            // Then
            factory.Verify();
        }

        [Fact]
        public void Should_Show_Usage_Information_If_Options_Are_Null()
        {
            // Given
            var log = new Mock<IConsoleBuildLog>().Object;
            var console = new Mock<IConsoleWriter>().Object;
            var parser = new Mock<IArgumentParser>();
            parser.Setup(x => x.Parse(It.IsAny<string[]>()))
                .Returns(() => null);

            var factory = new Mock<ICommandFactory>();
            factory.Setup(x => x.CreateHelpCommand(It.IsAny<LuntOptions>()))
                .Returns(() => new FakeCommand()).Verifiable();

            // When
            new LuntApplication(console, log, parser.Object, factory.Object)
                .Run(new string[] { });

            // Then
            factory.Verify();
        }

        [Fact]
        public void Should_Show_Usage_Information_If_No_Options_Are_Set()
        {
            // Given
            var log = new Mock<IConsoleBuildLog>().Object;
            var console = new Mock<IConsoleWriter>().Object;
            var parser = new Mock<IArgumentParser>();
            parser.Setup(x => x.Parse(It.IsAny<string[]>()))
                .Returns(() => new LuntOptions());

            var factory = new Mock<ICommandFactory>();
            factory.Setup(x => x.CreateHelpCommand(It.IsAny<LuntOptions>()))
                .Returns(() => new FakeCommand()).Verifiable();

            // When
            new LuntApplication(console, log, parser.Object, factory.Object)
                .Run(new string[] { });

            // Then
            factory.Verify();
        }

        [Fact]
        public void Should_Catch_Exceptions_In_Commands()
        {
            // Given
            var log = new Mock<IConsoleBuildLog>().Object;
            var console = new Mock<IConsoleWriter>().Object;
            var parser = new ArgumentParser(log);
            var factory = new Mock<ICommandFactory>();
            factory.Setup(x => x.CreateVersionCommand(It.IsAny<LuntOptions>()))
                .Returns(() => new FakeCommand(() => { throw new InvalidOperationException(); }))
                .Verifiable();

            // When
            var result = new LuntApplication(console, log, parser, factory.Object)
                .Run(new[] { "-version" });

            // Then
            Assert.Equal(1, result);
            factory.Verify();
        }  
    }
}