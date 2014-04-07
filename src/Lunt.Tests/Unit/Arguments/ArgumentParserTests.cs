﻿// ﻿
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
using Lunt.Diagnostics;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Lunt.Tests.Unit.Arguments
{
    public class ArgumentParserTests
    {
        public class TheParseMethod
        {
            [Fact]
            public void Can_Parse_Empty_Parameters()
            {
                // Given
                var log = new Mock<IBuildLog>();
                var parser = new ArgumentParser(log.Object);

                // When
                var result = parser.Parse(new string[] {});

                // Then
                Assert.NotNull(result);
            }

            [Fact]
            public void Should_Log_And_Return_Null_If_Parser_Encounters_Unknown_Switch()
            {
                // Given
                var log = new Mock<IBuildLog>();
                var parser = new ArgumentParser(log.Object);

                // When
                var result = parser.Parse(new[] {"-unknown"});

                // Then
                Assert.Null(result);
                log.Verify(x => x.Write(Verbosity.Quiet, LogLevel.Error, "Unknown option: unknown"));
            }

            [Theory]
            [InlineData("-input=test", "test")]
            [InlineData("-INPUT=test", "test")]
            [InlineData("-input=\"test", "\"test")]
            [InlineData("-input=\"test\"", "test")]
            [InlineData("-i=test", "test")]
            [InlineData("-I=test", "test")]
            [InlineData("-i=\"test", "\"test")]
            [InlineData("-i=\"test\"", "test")]
            public void Can_Parse_Input_Directory(string input, string value)
            {
                // Given
                var log = new Mock<IBuildLog>();
                var parser = new ArgumentParser(log.Object);

                // When
                var result = parser.Parse(new[] {input});

                // Then
                Assert.NotNull(result.InputDirectory);
                Assert.Equal(value, result.InputDirectory.FullPath);
            }

            [Theory]
            [InlineData("-output=test", "test")]
            [InlineData("-OUTPUT=test", "test")]
            [InlineData("-output=\"test", "\"test")]
            [InlineData("-output=\"test\"", "test")]
            [InlineData("-o=test", "test")]
            [InlineData("-O=test", "test")]
            [InlineData("-o=\"test", "\"test")]
            [InlineData("-o=\"test\"", "test")]
            public void Can_Parse_Output_Directory(string input, string value)
            {
                // Given
                var log = new Mock<IBuildLog>();
                var parser = new ArgumentParser(log.Object);

                // When
                var result = parser.Parse(new[] {input});

                // Then
                Assert.NotNull(result.OutputDirectory);
                Assert.Equal(value, result.OutputDirectory.FullPath);
            }

            [Theory]
            [InlineData("-probing=test", "test")]
            [InlineData("-PROBING=test", "test")]
            [InlineData("-probing=\"test", "\"test")]
            [InlineData("-probing=\"test\"", "test")]
            [InlineData("-p=test", "test")]
            [InlineData("-P=test", "test")]
            [InlineData("-p=\"test", "\"test")]
            [InlineData("-p=\"test\"", "test")]
            public void Can_Parse_Probing_Directory(string input, string value)
            {
                // Given
                var log = new Mock<IBuildLog>();
                var parser = new ArgumentParser(log.Object);

                // When
                var result = parser.Parse(new[] { input });

                // Then
                Assert.NotNull(result.ProbingDirectory);
                Assert.Equal(value, result.ProbingDirectory.FullPath);
            }


            [Theory]
            [InlineData("-verbosity=quiet", Verbosity.Quiet)]
            [InlineData("-verbosity=minimal", Verbosity.Minimal)]
            [InlineData("-verbosity=normal", Verbosity.Normal)]
            [InlineData("-verbosity=verbose", Verbosity.Verbose)]
            [InlineData("-verbosity=diagnostic", Verbosity.Diagnostic)]
            [InlineData("-verbosity=q", Verbosity.Quiet)]
            [InlineData("-verbosity=m", Verbosity.Minimal)]
            [InlineData("-verbosity=n", Verbosity.Normal)]
            [InlineData("-verbosity=v", Verbosity.Verbose)]
            [InlineData("-verbosity=d", Verbosity.Diagnostic)]
            [InlineData("-v=quiet", Verbosity.Quiet)]
            [InlineData("-v=minimal", Verbosity.Minimal)]
            [InlineData("-v=normal", Verbosity.Normal)]
            [InlineData("-v=verbose", Verbosity.Verbose)]
            [InlineData("-v=diagnostic", Verbosity.Diagnostic)]
            [InlineData("-v=q", Verbosity.Quiet)]
            [InlineData("-v=m", Verbosity.Minimal)]
            [InlineData("-v=n", Verbosity.Normal)]
            [InlineData("-v=v", Verbosity.Verbose)]
            [InlineData("-v=d", Verbosity.Diagnostic)]
            public void Can_Parse_Verbosity(string input, Verbosity value)
            {
                // Given
                var log = new Mock<IBuildLog>();
                var parser = new ArgumentParser(log.Object);

                // When
                var result = parser.Parse(new[] {input});

                // Then
                Assert.Equal(value, result.Verbosity);
            }

            [Theory]
            [InlineData("-help")]
            [InlineData("-h")]
            [InlineData("-?")]
            public void Can_Parse_Help(string input)
            {
                // Given
                var log = new Mock<IBuildLog>();
                var parser = new ArgumentParser(log.Object);

                // When
                var result = parser.Parse(new[] {input});

                // Then
                Assert.True(result.ShowHelp);
            }

            [Theory]
            [InlineData("-version")]
            [InlineData("-ver")]
            public void Can_Parse_Version(string input)
            {
                // Given
                var log = new Mock<IBuildLog>();
                var parser = new ArgumentParser(log.Object);

                // When
                var result = parser.Parse(new[] {input});

                // Then
                Assert.True(result.ShowVersion);
            }

            [Theory]
            [InlineData("-rebuild")]
            [InlineData("-r")]
            public void Can_Parse_Rebuild(string input)
            {
                // Given
                var log = new Mock<IBuildLog>();
                var parser = new ArgumentParser(log.Object);

                // When
                var result = parser.Parse(new[] {input});

                // Then
                Assert.True(result.Rebuild);
            }

            [Theory]
            [InlineData("build.config")]
            [InlineData("-verbosity=quiet build.config")]
            public void Can_Parse_Build_Configuration(string input)
            {
                // Given
                var log = new Mock<IBuildLog>();
                var parser = new ArgumentParser(log.Object);
                var arguments = input.Split(new[] {' '}, StringSplitOptions.None);

                // When
                var result = parser.Parse(arguments);

                // Then
                Assert.NotNull(result.BuildConfiguration);
                Assert.Equal("build.config", result.BuildConfiguration.FullPath);
            }

            [Theory]
            [InlineData("/home/test/build.config")]
            [InlineData("\"/home/test/build.config\"")]
            public void Can_Parse_Build_Configuration_With_Unix_Path(string input)
            {
                // Given
                var log = new Mock<IBuildLog>();
                var parser = new ArgumentParser(log.Object);
                var arguments = input.Split(new[] {' '}, StringSplitOptions.None);

                // When
                var result = parser.Parse(arguments);

                // Then
                Assert.NotNull(result);
                Assert.NotNull(result.BuildConfiguration);
                Assert.Equal("/home/test/build.config", result.BuildConfiguration.FullPath);
            }
        }
    }
}