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
using System.Xml.Linq;
using Lunt.Tests.Extensions;
using Lunt.Tests.Fakes;
using Xunit;

namespace Lunt.Tests.Unit
{
    public class BuildConfigurationXmlReaderTests
    {
        public class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_File_System_Is_Null()
            {
                // Givem, When
                var result = Record.Exception(() => new BuildConfigurationXmlReader(null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("fileSystem", ((ArgumentNullException)result).ParamName);
            }
        }

        public class TheReadMethod
        {
            [Fact]
            public void Should_Throw_If_File_Do_Not_Exist()
            {
                // Given
                var fileSystem = new FakeFileSystem();
                fileSystem.GetFile("/assets/build.config");
                var service = new BuildConfigurationXmlReader(fileSystem);

                // When                
                var result = Record.Exception(() => service.Read("/assets/build.config"));

                // Then
                Assert.IsType<LuntException>(result);
                Assert.Equal("Build configuration file do not exist.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Configuration_Is_Invalid()
            {
                // Given
                var fileSystem = new FakeFileSystem();
                const string xml = @"<build><asset /></build>";
                fileSystem.GetFile("/assets/build.config").Create(xml);
                var service = new BuildConfigurationXmlReader(fileSystem);

                // When
                var result = Record.Exception(() => service.Read("/assets/build.config"));

                // Then
                Assert.IsType<LuntException>(result);
                Assert.Equal("Could not load build configuration.", result.Message);
            }

            [Fact]
            public void Should_Return_Configuration_If_Valid()
            {
                // Given
                var fileSystem = new FakeFileSystem();
                const string xml = @"<build><asset path=""asset.txt""></asset></build>";
                fileSystem.GetFile("/assets/build.config").Create(xml);
                var service = new BuildConfigurationXmlReader(fileSystem);

                // When
                var result = service.Read("/assets/build.config");

                // Then
                Assert.NotNull(result);
                Assert.Equal(1, result.Assets.Count);
                Assert.IsType<AssetDefinition>(result.Assets[0]);
                Assert.Equal("asset.txt", result.Assets[0].Path.FullPath);
            }

            [Fact]
            public void Document_Passed_To_Constructor_Cannot_Be_Null()
            {
                // Given
                var reader = new BuildConfigurationXmlReader(new FakeFileSystem());

                // When
                var result = Record.Exception(() => reader.Read((XDocument)null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("document", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void XML_Must_Contain_Build_Element()
            {
                // Given
                var reader = new BuildConfigurationXmlReader(new FakeFileSystem());

                const string xml = @"<Hello World=""True""/>";
                XDocument document = XDocument.Parse(xml);

                // When
                var result = Record.Exception(() => reader.Read(document));

                // Then
                Assert.IsType<LuntException>(result);
                Assert.Equal("Configuration XML is missing 'build' element.", result.Message);
            }

            [Fact]
            public void Build_Element_Is_Case_Insensitive()
            {
                // Given
                var reader = new BuildConfigurationXmlReader(new FakeFileSystem());

                const string xml = @"<BUILD/>";
                XDocument document = XDocument.Parse(xml);

                // When
                var result = reader.Read(document);

                // Then
                Assert.NotNull(result);
                Assert.Equal(0, result.Assets.Count);
            }

            [Fact]
            public void Path_Attribute_Of_Content_Cannot_Be_Empty()
            {
                // Given
                var reader = new BuildConfigurationXmlReader(new FakeFileSystem());

                const string xml = @"<build><asset path=""""/></build>";
                XDocument document = XDocument.Parse(xml);

                // When
                var result = Record.Exception(() => reader.Read(document));

                // Then
                Assert.IsType<LuntException>(result);
                Assert.Equal("Asset element contains empty 'path' attribute.", result.Message);
            }

            [Fact]
            public void Can_Read_Asset_Element()
            {
                // Given
                var reader = new BuildConfigurationXmlReader(new FakeFileSystem());

                const string xml = @"<build><asset path=""texture.png""/></build>";
                XDocument document = XDocument.Parse(xml);

                // When
                var result = reader.Read(document);

                // Then
                Assert.Equal(1, result.Assets.Count);
                Assert.IsType<AssetDefinition>(result.Assets[0]);
                Assert.Equal("texture.png", result.Assets[0].Path.FullPath);
            }

            [Fact]
            public void Asset_Element_Is_Case_Insensitive()
            {
                // Given
                var reader = new BuildConfigurationXmlReader(new FakeFileSystem());

                const string xml = @"<build><ASSET path=""texture.png""/></build>";
                XDocument document = XDocument.Parse(xml);

                // When
                var result = reader.Read(document);

                // Then
                Assert.Equal(1, result.Assets.Count);
                Assert.IsType<AssetDefinition>(result.Assets[0]);
                Assert.Equal("texture.png", result.Assets[0].Path.FullPath);
            }

            [Fact]
            public void Path_Attribute_Of_Asset_Element_Is_Case_Insensitive()
            {
                // Given
                var reader = new BuildConfigurationXmlReader(new FakeFileSystem());

                const string xml = @"<build><asset PATH=""texture.png""/></build>";
                XDocument document = XDocument.Parse(xml);

                // When
                var result = reader.Read(document);

                // Then
                Assert.Equal(1, result.Assets.Count);
                Assert.IsType<AssetDefinition>(result.Assets[0]);
                Assert.Equal("texture.png", result.Assets[0].Path.FullPath);
            }

            [Fact]
            public void Asset_Element_Can_Have_Processor_Attribute()
            {
                // Given
                var reader = new BuildConfigurationXmlReader(new FakeFileSystem());

                const string xml = @"<build><asset path=""texture.png"" processor=""MyProcessor""/></build>";
                XDocument document = XDocument.Parse(xml);

                // When
                var result = reader.Read(document);

                // Then
                Assert.Equal(1, result.Assets.Count);
                Assert.Equal("MyProcessor", result.Assets[0].ProcessorName);
            }

            [Fact]
            public void Processor_Attribute_Of_Asset_Element_Is_Case_Insensitive()
            {
                // Given
                var reader = new BuildConfigurationXmlReader(new FakeFileSystem());

                const string xml = @"<build><asset path=""texture.png"" PROCESSOR=""MyProcessor""/></build>";
                XDocument document = XDocument.Parse(xml);

                // When
                var result = reader.Read(document);

                // Then
                Assert.Equal(1, result.Assets.Count);
                Assert.Equal("MyProcessor", result.Assets[0].ProcessorName);
            }

            [Fact]
            public void Asset_Processor_Attribute_Value_Can_Not_Be_Empty()
            {
                // Given
                var reader = new BuildConfigurationXmlReader(new FakeFileSystem());

                const string xml = @"<build><asset path=""texture.png"" PROCESSOR=""""/></build>";
                XDocument document = XDocument.Parse(xml);

                // When
                var result = reader.Read(document);

                // Then
                Assert.Equal(1, result.Assets.Count);
                Assert.Equal(null, result.Assets[0].ProcessorName);
            }

            [Fact]
            public void Can_Read_Metadata()
            {
                // Given
                var reader = new BuildConfigurationXmlReader(new FakeFileSystem());

                const string xml = @"<build><asset path=""texture.png""><metadata key=""key"">value</metadata></asset></build>";
                XDocument document = XDocument.Parse(xml);

                // When
                var result = reader.Read(document);

                // Then
                Assert.True(result.Assets[0].Metadata.ContainsKey("key"));
                Assert.Equal("value", result.Assets[0].Metadata["key"]);
            }

            [Fact]
            public void Metadata_Element_Must_Contain_Key_Attribute()
            {
                // Given
                var reader = new BuildConfigurationXmlReader(new FakeFileSystem());

                const string xml = @"<build><asset path=""texture.png""><metadata>value</metadata></asset></build>";
                XDocument document = XDocument.Parse(xml);

                // When
                var result = Record.Exception(() => reader.Read(document));

                // Then
                Assert.IsType<LuntException>(result);
                Assert.Equal("Metadata element is missing 'key' attribute.", result.Message);
            }

            [Fact]
            public void Key_Attribute_Of_Metadata_Can_Not_Be_Empty()
            {
                // Given
                var reader = new BuildConfigurationXmlReader(new FakeFileSystem());

                const string xml = @"<build><asset path=""texture.png""><metadata key="""">value</metadata></asset></build>";
                XDocument document = XDocument.Parse(xml);

                // When
                var result = Record.Exception(() => reader.Read(document));

                // Then
                Assert.IsType<LuntException>(result);
                Assert.Equal("Metadata element contains empty 'key' attribute.", result.Message);
            }

            [Fact]
            public void Path_Attribute_With_No_Wildcards_Is_Treated_Like_A_Regular_Path()
            {
                // Given
                var reader = new BuildConfigurationXmlReader(new FakeFileSystem());

                const string xml = @"<build><asset path=""Text/asset.txt"" /></build>";
                XDocument document = XDocument.Parse(xml);

                // When
                var result = reader.Read(document);

                // Then
                Assert.Equal(1, result.Assets.Count);
                Assert.False(result.Assets[0].IsGlob);
            }

            [Fact]
            public void Path_Attribute_With_Directory_Wildcard_Is_Treated_Like_A_Glob()
            {
                // Given
                var reader = new BuildConfigurationXmlReader(new FakeFileSystem());

                const string xml = @"<build><asset path=""Text/**/asset.txt"" /></build>";
                XDocument document = XDocument.Parse(xml);

                // When
                var result = reader.Read(document);

                // Then
                Assert.Equal(1, result.Assets.Count);
                Assert.True(result.Assets[0].IsGlob);
            }

            [Fact]
            public void Path_Attribute_With_Character_Wildcard_Is_Treated_Like_A_Glob()
            {
                // Given
                var reader = new BuildConfigurationXmlReader(new FakeFileSystem());

                const string xml = @"<build><asset path=""Text/a??et.txt"" /></build>";
                XDocument document = XDocument.Parse(xml);

                // When
                var result = reader.Read(document);

                // Then
                Assert.Equal(1, result.Assets.Count);
                Assert.True(result.Assets[0].IsGlob);
            }
        }
    }
}