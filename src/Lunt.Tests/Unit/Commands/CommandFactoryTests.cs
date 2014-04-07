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
using Lunt.Commands;
using Lunt.Diagnostics;
using Lunt.IO;
using Lunt.Runtime;
using Lunt.Tests.Fakes;
using Moq;
using Xunit;

namespace Lunt.Tests.Unit.Commands
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

                CommandFactory factory = new CommandFactory(log, console, environment, scannerFactory);

                // When
                var result = factory.CreateHelpCommand(new LuntOptions());

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

                CommandFactory factory = new CommandFactory(log, console, environment, scannerFactory);

                // When
                var result = factory.CreateVersionCommand(new LuntOptions());

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

                CommandFactory factory = new CommandFactory(log, console, environment.Object, scannerFactory.Object);

                // When
                var result = factory.CreateBuildCommand(new LuntOptions());

                // Then
                Assert.IsType<BuildCommand>(result);
            }
        }
    }
}