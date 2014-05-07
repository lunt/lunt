using System;
using System.Collections.Generic;
using Lunt.Diagnostics;
using Lunt.IO;
using Lunt.Runtime;
using Lunt.Testing;
using Lunt.Tests.Fixtures;
using Moq;
using Xunit;

namespace Lunt.Tests.Unit
{
    public class BuildEngineTests
    {
        public class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var scanner = new Mock<IPipelineScanner>().Object;
                var hashComputer = new Mock<IHashComputer>().Object;
                var log = new Mock<IBuildLog>().Object;

                // When
                var result = Record.Exception(() => new BuildEngine(null, scanner, hashComputer, log));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("environment", ((ArgumentNullException) result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_File_System_From_Environment_Is_Null()
            {
                // Given
                var environment = new Mock<IBuildEnvironment>().Object;
                var scanner = new Mock<IPipelineScanner>().Object;
                var hashComputer = new Mock<IHashComputer>().Object;
                var log = new Mock<IBuildLog>().Object;

                // When
                var result = Record.Exception(() => new BuildEngine(environment, scanner, hashComputer, log));

                // Then
                Assert.IsType<ArgumentException>(result);
                Assert.True(result.Message.StartsWith("The build environment's file system was null."));
            }

            [Fact]
            public void Should_Throw_If_Scanner_Is_Null()
            {
                // Given
                var environment = new FakeBuildEnvironment();
                var hashComputer = new Mock<IHashComputer>().Object;
                var log = new Mock<IBuildLog>().Object;

                // When
                var result = Record.Exception(() => new BuildEngine(environment, null, hashComputer, log));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("scanner", ((ArgumentNullException) result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Build_Log_Is_Null()
            {
                // Given
                var environment = new FakeBuildEnvironment();
                var scanner = new Mock<IPipelineScanner>().Object;
                var hashComputer = new Mock<IHashComputer>().Object;

                // When
                var result = Record.Exception(() => new BuildEngine(environment, scanner, hashComputer, null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("log", ((ArgumentNullException) result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Importer_Has_No_Attribute()
            {
                // Given
                var facade = new BuildEngineFactory();
                facade.Components.Importers.Add(new FakeImporter<int>((c, f) => 0));

                // When
                var result = Record.Exception(() => facade.CreateBuildEngine());

                // Then
                Assert.IsType<LuntException>(result);
                Assert.Equal("The importer Lunt.Testing.FakeImporter`1" +
                             "[[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]" +
                             " has not been decorated with an importer attribute.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Importer_Is_Missing_Extension()
            {
                // Given
                var facade = new BuildEngineFactory();
                facade.Components.Importers.Add(FakeImporter<int>.Mock((c, f) => 0, null));

                // When
                var result = Record.Exception(() => facade.CreateBuildEngine());

                // Then
                Assert.IsType<LuntException>(result);
                Assert.Equal("The importer Castle.Proxies.FakeImporter`1Proxy has not been associated with any file extensions.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Adding_Two_Importers_Handling_Same_Type()
            {
                // Given
                var facade = new BuildEngineFactory();
                facade.Components.Importers.Add(FakeImporter<int>.Mock((c, f) => 0, ".asset"));
                facade.Components.Importers.Add(FakeImporter<int>.Mock((c, f) => 0, ".asset"));

                // When
                var result = Record.Exception(() => facade.CreateBuildEngine());

                // Then
                Assert.IsType<LuntException>(result);
                Assert.Equal("More than one importer has been associated with the file extension '.asset'.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Default_Processor_For_Importer_Is_Not_A_Processor()
            {
                // Given
                var facade = new BuildEngineFactory();
                facade.Components.Importers.Add(FakeImporter<int>.Mock((c, f) => 0, ".asset", defaultProcessor: typeof (string)));

                // When
                var result = Record.Exception(() => facade.CreateBuildEngine());

                // Then
                Assert.IsType<LuntException>(result);
                Assert.Equal("The default processor (System.String) referenced by " +
                             "Castle.Proxies.FakeImporter`1Proxy is not a processor.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Default_Processor_For_Importer_Is_Abstract()
            {
                // Given
                var facade = new BuildEngineFactory();
                facade.Components.Importers.Add(FakeImporter<int>.Mock((c, f) => 0, ".asset", defaultProcessor: typeof (Processor<>)));

                // When
                var result = Record.Exception(() => facade.CreateBuildEngine());

                // Then
                Assert.IsType<LuntException>(result);
                Assert.Equal("The default processor (Lunt.Processor`1) referenced by " +
                             "Castle.Proxies.FakeImporter`1Proxy is abstract.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Processor_Source_Type_Is_Null()
            {
                // Given
                var facade = new BuildEngineFactory();
                var processor = new FakeProcessor(null, sourceType: null, targetType: typeof (int));
                facade.Components.Processors.Add(processor);

                // When
                var result = Record.Exception(() => facade.CreateBuildEngine());

                // Then
                Assert.IsType<LuntException>(result);
                Assert.Equal("The processor Lunt.Testing.FakeProcessor has no source type.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Processor_Target_Type_Is_Null()
            {
                // Given
                var facade = new BuildEngineFactory();
                var processor = new FakeProcessor(null, sourceType: typeof (int), targetType: null);
                facade.Components.Processors.Add(processor);

                // When
                var result = Record.Exception(() => facade.CreateBuildEngine());

                // Then
                Assert.IsType<LuntException>(result);
                Assert.Equal("The processor Lunt.Testing.FakeProcessor has no target type.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Writer_Target_Type_Is_Null()
            {
                // Given
                var facade = new BuildEngineFactory();
                var writer = new FakeWriter(null, targetType: null);
                facade.Components.Writers.Add(writer);

                // When
                var result = Record.Exception(() => facade.CreateBuildEngine());

                // Then
                Assert.IsType<LuntException>(result);
                Assert.Equal("The writer Lunt.Testing.FakeWriter has no target type.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Adding_Two_Writers_Handling_Same_Target_Type()
            {
                // Given
                var facade = new BuildEngineFactory();
                facade.Components.Writers.Add(new FakeWriter<int>((c, f, v) => { }));
                facade.Components.Writers.Add(FakeWriter<Int32>.Mock((c, f, v) => { }));

                // When
                var result = Record.Exception(() => facade.CreateBuildEngine());

                // Then
                Assert.IsType<LuntException>(result);
                Assert.Equal("More than one writer has been associated with the type System.Int32.", result.Message);
            }
        }

        public class TheBuildMethod
        {
            [Fact]
            public void Should_Throw_If_Build_Configuration_Is_Null()
            {
                // Given
                var environment = new FakeBuildEnvironment();
                var component = new Mock<IPipelineScanner>().Object;
                var hashComputer = new Mock<IHashComputer>().Object;
                var log = new Mock<IBuildLog>().Object;
                var kernel = new BuildEngine(environment, component, hashComputer, log);

                // When
                var result = Record.Exception(() => kernel.Build(null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("configuration", ((ArgumentNullException) result).ParamName);
            }

            [Fact]
            public void Should_Return_Empty_Manifest_If_No_Assets_Were_Provided()
            {
                // Given
                var facade = new BuildEngineFactory();
                facade.FileSystem.GetCreatedFile("/input/simple.asset");
                facade.CreateBuildEngine();

                // When
                var result = facade.Engine.Build(facade.Configuration);

                // Then
                Assert.Equal(0, result.Items.Count);
            }

            [Fact]
            public void Should_Throw_If_Input_Directory_Is_Null()
            {
                // Given
                var facade = new BuildEngineFactory();
                facade.Configuration.InputDirectory = null;
                facade.CreateBuildEngine();

                // When
                var result = Record.Exception(() => facade.Engine.Build(facade.Configuration));

                // Then
                Assert.IsType<LuntException>(result);
                Assert.Equal("Input directory has not been set.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Output_Directory_Is_Null()
            {
                // Given
                var facade = new BuildEngineFactory();
                facade.Configuration.OutputDirectory = null;
                facade.CreateBuildEngine();

                // When
                var result = Record.Exception(() => facade.Engine.Build(facade.Configuration));

                // Then
                Assert.IsType<LuntException>(result);
                Assert.Equal("Output directory has not been set.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Input_Directory_Is_Relative()
            {
                // Given
                var facade = new BuildEngineFactory();
                facade.Configuration.InputDirectory = "relative-input";
                facade.CreateBuildEngine();

                // When
                var result = Record.Exception(() => facade.Engine.Build(facade.Configuration));

                // Then
                Assert.IsType<LuntException>(result);
                Assert.Equal("Input directory cannot be relative.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Output_Directory_Is_Relative()
            {
                // Given
                var facade = new BuildEngineFactory();
                facade.Configuration.OutputDirectory = "relative-output";
                facade.CreateBuildEngine();

                // When
                var result = Record.Exception(() => facade.Engine.Build(facade.Configuration));

                // Then
                Assert.IsType<LuntException>(result);
                Assert.Equal("Output directory cannot be relative.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Input_Directory_Does_Not_Exist()
            {
                // Given
                var facade = new BuildEngineFactory();
                facade.FileSystem.GetDirectory("/input");
                facade.FileSystem.DeleteDirectory("/input");
                facade.CreateBuildEngine();

                // When
                var result = Record.Exception(() => facade.Engine.Build(facade.Configuration));

                // Then
                Assert.IsType<LuntException>(result);
                Assert.Equal("Input directory '/input' does not exist.", result.Message);
            }

            [Fact]
            public void Returns_Empty_Manifest_If_No_Assets_Were_Provided_In_Configuration()
            {
                // Given
                var facade = new BuildEngineFactory();
                facade.Configuration.Assets.Clear();
                facade.CreateBuildEngine();

                // When
                var result = facade.Engine.Build(facade.Configuration);

                // Then
                Assert.Equal(0, result.Items.Count);
            }

            [Fact]
            public void Returns_Failure_If_Asset_File_Is_Missing()
            {
                // Given
                var facade = new BuildEngineFactory();
                facade.Configuration.Assets.Add(new AssetDefinition("simple.asset"));
                facade.CreateBuildEngine();

                // When
                var result = facade.Engine.Build(facade.Configuration);

                // Then
                Assert.Equal(1, result.Items.Count);
                Assert.Equal(AssetBuildStatus.Failure, result.Items[0].Status);
                Assert.Equal("Could not find the file '/input/simple.asset'.", result.Items[0].Message);
            }

            [Fact]
            public void Returns_Failure_If_Asset_Has_No_File_Extension()
            {
                // Given
                var facade = new BuildEngineFactory();
                facade.Configuration.Assets.Add(new AssetDefinition("simple"));
                facade.CreateBuildEngine();

                // When
                var result = facade.Engine.Build(facade.Configuration);

                // Then
                Assert.Equal(1, result.Items.Count);
                Assert.Equal(AssetBuildStatus.Failure, result.Items[0].Status);
                Assert.Equal("The asset @simple has no file extension.", result.Items[0].Message);
            }

            [Fact]
            public void Returns_Failure_If_Importer_Is_Missing()
            {
                // Given
                var facade = new BuildEngineFactory();
                facade.FileSystem.GetCreatedFile("/input/simple.asset");
                facade.Configuration.Assets.Add(new AssetDefinition("simple.asset"));
                facade.CreateBuildEngine();

                // When
                var result = facade.Engine.Build(facade.Configuration);

                // Then
                Assert.Equal(1, result.Items.Count);
                Assert.Equal(AssetBuildStatus.Failure, result.Items[0].Status);
                Assert.Equal("Could not find an importer for @simple.asset.", result.Items[0].Message);
            }

            [Fact]
            public void Returns_Failure_If_Imported_Data_Was_Null()
            {
                // Given
                var facade = new BuildEngineFactory();
                facade.FileSystem.GetCreatedFile("/input/simple.asset");
                facade.Configuration.Assets.Add(new AssetDefinition("simple.asset"));
                facade.Components.Importers.Add(FakeImporter<string>.Mock((c, f) => null, ".asset"));
                facade.CreateBuildEngine();

                // When
                var result = facade.Engine.Build(facade.Configuration);

                // Then
                Assert.Equal(1, result.Items.Count);
                Assert.Equal(AssetBuildStatus.Failure, result.Items[0].Status);
                Assert.Equal("Import of @simple.asset resulted in null.", result.Items[0].Message);
            }

            [Fact]
            public void Returns_Failure_If_Processed_Data_Was_Null()
            {
                // Given
                var facade = new BuildEngineFactory();
                facade.FileSystem.GetCreatedFile("/input/simple.asset");
                facade.Configuration.Assets.Add(new AssetDefinition("simple.asset"));

                var processor = new FakeProcessor<string, string>((c, v) => null);
                var importer = FakeImporter<string>.Mock((c, f) => "Hello", ".asset", defaultProcessor: processor.GetType());
                facade.Components.Importers.Add(importer);
                facade.Components.Processors.Add(processor);

                facade.CreateBuildEngine();

                // When
                var result = facade.Engine.Build(facade.Configuration);

                // Then
                Assert.Equal(1, result.Items.Count);
                Assert.Equal(AssetBuildStatus.Failure, result.Items[0].Status);
                Assert.Equal("Processing of @simple.asset resulted in null.", result.Items[0].Message);
            }

            [Fact]
            public void Returns_Failure_If_Processed_Data_Was_Not_Of_Expected_Type()
            {
                // Given
                var facade = new BuildEngineFactory();
                facade.FileSystem.GetCreatedFile("/input/simple.asset");
                facade.Configuration.Assets.Add(new AssetDefinition("simple.asset"));

                var processor = new FakeProcessor((c, f) => 42, typeof(string), typeof(string));
                var importer = FakeImporter<string>.Mock((c, f) => "Hello", ".asset", defaultProcessor: processor.GetType());
                facade.Components.Importers.Add(importer);
                facade.Components.Processors.Add(processor);

                facade.CreateBuildEngine();

                // When
                var result = facade.Engine.Build(facade.Configuration);

                // Then
                Assert.Equal(1, result.Items.Count);
                Assert.Equal(AssetBuildStatus.Failure, result.Items[0].Status);
                Assert.Equal("Value returned from processor for @simple.asset does not match expected target type (System.String).", result.Items[0].Message);
            }

            [Fact]
            public void Returns_Failure_If_Writer_Is_Missing()
            {
                // Given
                var facade = new BuildEngineFactory();
                facade.FileSystem.GetCreatedFile("/input/simple.asset");
                facade.Configuration.Assets.Add(new AssetDefinition("simple.asset"));
                facade.Components.Importers.Add(FakeImporter<string>.Mock((c, f) => "Hello", ".asset"));
                facade.CreateBuildEngine();

                // When
                var result = facade.Engine.Build(facade.Configuration);

                // Then
                Assert.Equal(1, result.Items.Count);
                Assert.Equal(AssetBuildStatus.Failure, result.Items[0].Status);
                Assert.Equal("Could not find a writer for @simple.asset.", result.Items[0].Message);
            }

            [Fact]
            public void Should_Use_Asset_Processor_If_Present()
            {
                var result = false;

                // Given
                var facade = new BuildEngineFactory();
                facade.FileSystem.GetCreatedFile("/input/simple.asset");

                var asset = new AssetDefinition("simple.asset");
                asset.ProcessorName = "MyProcessor";
                facade.Configuration.Assets.Add(asset);

                var defaultProcessor = new FakeProcessor<string, string>((c, v) => null);
                var importer = FakeImporter<string>.Mock((c, f) => "Hello", ".asset", defaultProcessor: defaultProcessor.GetType());
                facade.Components.Importers.Add(importer);
                facade.Components.Processors.Add(defaultProcessor);
                facade.Components.Processors.Add(FakeProcessor<string, string>.Mock((c, v) =>
                {
                    result = true;
                    return v;
                }, "MyProcessor"));

                facade.CreateBuildEngine();

                // When
                facade.Engine.Build(facade.Configuration);

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Returns_Failure_If_Asset_Processor_Is_Missing()
            {
                // Given
                var facade = new BuildEngineFactory();
                facade.FileSystem.GetCreatedFile("/input/simple.asset");

                var asset = new AssetDefinition("simple.asset");
                asset.ProcessorName = "MyProcessor";
                facade.Configuration.Assets.Add(asset);

                var importer = FakeImporter<string>.Mock((c, f) => "Hello", ".asset");
                facade.Components.Importers.Add(importer);

                facade.CreateBuildEngine();

                // When
                var result = facade.Engine.Build(facade.Configuration);

                // Then
                Assert.Equal(1, result.Items.Count);
                Assert.Equal(AssetBuildStatus.Failure, result.Items[0].Status);
                Assert.Equal("Cannot process @simple.asset since the processor 'MyProcessor' wasn't found.", result.Items[0].Message);
            }

            [Fact]
            public void Should_Return_Failure_If_Imported_Data_Do_Not_Match_Processor_Source_Type()
            {
                // Given
                var facade = new BuildEngineFactory();
                facade.FileSystem.GetCreatedFile("/input/simple.asset");
                facade.Configuration.Assets.Add(new AssetDefinition("simple.asset"));

                var processor = new FakeProcessor<int, int>((c, v) => 9);
                var importer = FakeImporter<string>.Mock((c, f) => "Hello", ".asset", defaultProcessor: processor.GetType());
                facade.Components.Importers.Add(importer);
                facade.Components.Processors.Add(processor);

                facade.CreateBuildEngine();

                // When
                var result = facade.Engine.Build(facade.Configuration);

                // Then
                Assert.Equal(1, result.Items.Count);
                Assert.Equal(AssetBuildStatus.Failure, result.Items[0].Status);
                Assert.Equal("Cannot process @simple.asset since the data is of the wrong type (System.String). " +
                             "Processor expected System.Int32.", result.Items[0].Message);
            }

            [Fact]
            public void Should_Create_Target_Directory_If_Missing()
            {
                // Given
                var facade = new BuildEngineFactory();
                facade.FileSystem.GetCreatedFile("/input/assets/simple.asset");
                facade.Configuration.Assets.Add(new AssetDefinition("assets/simple.asset"));
                facade.Components.Importers.Add(FakeImporter<string>.Mock((c, f) => "Hello", ".asset"));
                facade.Components.Writers.Add(FakeWriter<string>.Mock((c, f, v) => { }));

                facade.CreateBuildEngine();

                // When
                facade.Engine.Build(facade.Configuration);
                var directory = facade.FileSystem.GetDirectory("/output/assets");

                // Then
                Assert.True(directory.Exists);
            }

            [Fact]
            public void Returns_Failure_If_Target_Directory_Can_Not_Be_Created()
            {
                // Given
                var facade = new BuildEngineFactory();
                facade.FileSystem.GetCreatedFile("/input/assets/simple.asset");
                facade.FileSystem.GetNonCreatableDirectory("/output/assets");
                facade.Configuration.Assets.Add(new AssetDefinition("assets/simple.asset"));
                facade.Components.Importers.Add(FakeImporter<string>.Mock((c, f) => "Hello", ".asset"));
                facade.Components.Writers.Add(FakeWriter<string>.Mock((c, f, v) => { }));

                facade.CreateBuildEngine();

                // When
                var result = facade.Engine.Build(facade.Configuration);

                // Then
                Assert.Equal(1, result.Items.Count);
                Assert.Equal(AssetBuildStatus.Failure, result.Items[0].Status);
                Assert.Equal("Could not create target directory '/output/assets'.", result.Items[0].Message);
            }

            [Fact]
            public void Should_Dispose_Imported_Data_After_Write()
            {
                // Given
                var data = new Mock<IDisposable>();
                data.Setup(d => d.Dispose()).Verifiable();

                var facade = new BuildEngineFactory();
                facade.FileSystem.GetCreatedFile("/input/assets/simple.asset");
                facade.FileSystem.GetNonCreatableDirectory("/output/assets");
                facade.Configuration.Assets.Add(new AssetDefinition("assets/simple.asset"));
                facade.Components.Importers.Add(FakeImporter<IDisposable>.Mock((c, f) => data.Object, ".asset"));
                facade.Components.Writers.Add(FakeWriter<IDisposable>.Mock((c, f, v) => { }));

                facade.CreateBuildEngine();

                // When
                facade.Engine.Build(facade.Configuration);

                // Then
                data.VerifyAll();
            }

            [Fact]
            public void Target_File_Should_Have_The_Correct_Extension()
            {
                // Given
                var facade = new BuildEngineFactory();
                facade.FileSystem.GetCreatedFile("/input/assets/simple.asset");
                facade.Configuration.Assets.Add(new AssetDefinition("assets/simple.asset"));
                facade.Components.Importers.Add(FakeImporter<string>.Mock((c, f) => string.Empty, ".asset"));
                facade.Components.Writers.Add(FakeWriter<string>.Mock((c, f, v) => f.Create().Close()));

                var target = facade.FileSystem.GetFile("/output/assets/simple.dat");

                facade.CreateBuildEngine();

                // When
                facade.Engine.Build(facade.Configuration);

                // Then
                Assert.True(target.Exists);
            }

            [Fact]
            public void Importer_Should_Receive_Expected_Context()
            {
                Context interceptedContext = null;

                // Given
                var facade = new BuildEngineFactory();
                var asset = new AssetDefinition("assets/simple.asset");
                facade.FileSystem.GetCreatedFile("/input/assets/simple.asset");
                facade.Configuration.Assets.Add(asset);
                facade.Components.Importers.Add(FakeImporter<string>.Mock((c, f) =>
                {
                    interceptedContext = c;
                    return string.Empty;
                }, ".asset"));
                facade.Components.Writers.Add(FakeWriter<string>.Mock((c, f, v) => { }));

                facade.CreateBuildEngine();

                // When
                facade.Engine.Build(facade.Configuration);

                // Then
                Assert.Equal(facade.FileSystem, interceptedContext.FileSystem);
                Assert.Equal(facade.Log, interceptedContext.Log);
                Assert.Equal(asset.Path.FullPath, interceptedContext.Asset.Path.FullPath);
            }

            [Fact]
            public void Processor_Should_Receive_Expected_Context()
            {
                Context interceptedContext = null;

                // Given
                var facade = new BuildEngineFactory();
                var asset = new AssetDefinition("assets/simple.asset");
                var processor = new FakeProcessor<string, string>((c, v) =>
                {
                    interceptedContext = c;
                    return v;
                });
                facade.FileSystem.GetCreatedFile("/input/assets/simple.asset");
                facade.Configuration.Assets.Add(asset);
                facade.Components.Importers.Add(FakeImporter<string>.Mock((c, f) => string.Empty, ".asset", processor.GetType()));
                facade.Components.Processors.Add(processor);
                facade.Components.Writers.Add(FakeWriter<string>.Mock((c, f, v) => { }));

                facade.CreateBuildEngine();

                // When
                facade.Engine.Build(facade.Configuration);

                // Then
                Assert.Equal(facade.FileSystem, interceptedContext.FileSystem);
                Assert.Equal(facade.Log, interceptedContext.Log);
                Assert.Equal(asset.Path.FullPath, interceptedContext.Asset.Path.FullPath);
            }

            [Fact]
            public void Writer_Should_Receive_Expected_Context()
            {
                Context interceptedContext = null;

                // Given
                var facade = new BuildEngineFactory();
                var asset = new AssetDefinition("assets/simple.asset");
                facade.FileSystem.GetCreatedFile("/input/assets/simple.asset");
                facade.FileSystem.GetCreatedFile("/output/assets/simple.dat");
                facade.Configuration.Assets.Add(asset);
                facade.Components.Importers.Add(FakeImporter<string>.Mock((c, f) => string.Empty, ".asset"));
                facade.Components.Writers.Add(FakeWriter<string>.Mock((c, f, v) => { interceptedContext = c; }));

                facade.CreateBuildEngine();

                // When
                facade.Engine.Build(facade.Configuration);

                // Then
                Assert.Equal(facade.FileSystem, interceptedContext.FileSystem);
                Assert.Equal(facade.Log, interceptedContext.Log);
                Assert.Equal(asset.Path.FullPath, interceptedContext.Asset.Path.FullPath);
            }

            [Fact]
            public void Build_Manifest_Should_Contain_Checksum_For_Source_File()
            {
                // Given
                var facade = new BuildEngineFactory();
                var asset = new AssetDefinition("assets/simple.asset");

                var content = new byte[] { 0, 1, 2, 3, 4, 5 };
                facade.FileSystem.GetFile("/input/assets/simple.asset").Create(content);

                var processor = new FakeProcessor<string, string>((c, v) => v);
                var importer = FakeImporter<string>.Mock((c, f) => string.Empty, ".asset", processor.GetType());
                var writer = FakeWriter<string>.Mock((c, f, v) => { });

                facade.Configuration.Assets.Add(asset);
                facade.Components.Importers.Add(importer);
                facade.Components.Processors.Add(processor);
                facade.Components.Writers.Add(writer);

                facade.CreateBuildEngine();

                // When
                var result = facade.Engine.Build(facade.Configuration);

                // Then
                Assert.NotEmpty(result.Items[0].Checksum);
            }

            [Fact]
            public void Build_Manifest_Should_Contain_File_Length_For_Source_File()
            {
                // Given
                var facade = new BuildEngineFactory();
                var asset = new AssetDefinition("assets/simple.asset");

                var content = new byte[] { 0, 1, 2, 3, 4, 5 };
                facade.FileSystem.GetFile("/input/assets/simple.asset").Create(content);

                var processor = new FakeProcessor<string, string>((c, v) => v);
                var importer = FakeImporter<string>.Mock((c, f) => string.Empty, ".asset", processor.GetType());
                var writer = FakeWriter<string>.Mock((c, f, v) => { });

                facade.Configuration.Assets.Add(asset);
                facade.Components.Importers.Add(importer);
                facade.Components.Processors.Add(processor);
                facade.Components.Writers.Add(writer);

                facade.CreateBuildEngine();

                // When
                var result = facade.Engine.Build(facade.Configuration);

                // Then
                Assert.Equal(content.Length, result.Items[0].Length);
            }

            [Fact]
            public void Should_Not_Rebuild_File_If_Nothing_Has_Changed()
            {
                // Given
                var asset = new AssetDefinition("assets/simple.asset");
                var data = BuildEngineFactory.DefaultContent;

                BuildManifest manifest;
                var facade = BuildEngineFactory.CreateWithPreviousManifest(asset, data, out manifest);

                facade.FileSystem.GetFile("/input/assets/simple.asset").Create(BuildEngineFactory.DefaultContent);
                facade.FileSystem.GetCreatedFile("/output/assets/simple.dat");

                facade.Configuration.Incremental = true;
                facade.CreateBuildEngine();

                // When
                var result = facade.Engine.Build(facade.Configuration, manifest);

                // Then
                Assert.True(facade.Configuration.Incremental);
                Assert.Equal(AssetBuildStatus.Skipped, result.Items[0].Status);
            }

            [Fact]
            public void Should_Rebuild_Asset_If_Target_File_Do_Not_Exist()
            {
                // Given
                var asset = new AssetDefinition("assets/simple.asset");
                var data = BuildEngineFactory.DefaultContent;

                BuildManifest manifest;
                var facade = BuildEngineFactory.CreateWithPreviousManifest(asset, data, out manifest);

                manifest.Items[0].Length = data.Length - 1;

                facade.FileSystem.GetFile("/input/assets/simple.asset").Create(BuildEngineFactory.DefaultContent);

                facade.Configuration.Incremental = true;
                facade.CreateBuildEngine();

                // When
                var result = facade.Engine.Build(facade.Configuration, manifest);

                // Then
                Assert.True(facade.Configuration.Incremental);
                Assert.Equal(AssetBuildStatus.Success, result.Items[0].Status);
                Assert.True(facade.Log.Messages.Contains("Target file does not exist. Rebuilding asset."));
            }

            [Fact]
            public void Should_Rebuild_Asset_If_Source_File_Length_Have_Changed()
            {
                // Given
                var asset = new AssetDefinition("assets/simple.asset");
                var data = BuildEngineFactory.DefaultContent;

                BuildManifest manifest;
                var facade = BuildEngineFactory.CreateWithPreviousManifest(asset, data, out manifest);

                manifest.Items[0].Length = data.Length - 1;

                facade.FileSystem.GetFile("/input/assets/simple.asset").Create(BuildEngineFactory.DefaultContent);
                facade.FileSystem.GetCreatedFile("/output/assets/simple.dat");

                facade.Configuration.Incremental = true;
                facade.CreateBuildEngine();

                // When
                var result = facade.Engine.Build(facade.Configuration, manifest);

                // Then
                Assert.True(facade.Configuration.Incremental);
                Assert.Equal(AssetBuildStatus.Success, result.Items[0].Status);
                Assert.True(facade.Log.Messages.Contains("Source file size have changed. Rebuilding asset."));
            }

            [Fact]
            public void Should_Rebuild_Asset_If_Source_File_Checksum_Have_Changed()
            {
                // Given
                var asset = new AssetDefinition("assets/simple.asset");
                var data = BuildEngineFactory.DefaultContent;

                BuildManifest manifest;
                var facade = BuildEngineFactory.CreateWithPreviousManifest(asset, data, out manifest);

                manifest.Items[0].Checksum = "ABCDEFGHIJKLMN";

                facade.FileSystem.GetFile("/input/assets/simple.asset").Create(BuildEngineFactory.DefaultContent);
                facade.FileSystem.GetCreatedFile("/output/assets/simple.dat");

                facade.Configuration.Incremental = true;
                facade.CreateBuildEngine();

                // When
                var result = facade.Engine.Build(facade.Configuration, manifest);

                // Then
                Assert.True(facade.Configuration.Incremental);
                Assert.Equal(AssetBuildStatus.Success, result.Items[0].Status);
                Assert.True(facade.Log.Messages.Contains("Source file checksum have changed. Rebuilding asset."));
            }

            [Fact]
            public void Should_Rebuild_Asset_If_Metadata_Key_Count_Have_Changed()
            {
                // Given
                var metadata = new Dictionary<string, string> {{"Key", "Value"}};
                var asset = new AssetDefinition("assets/simple.asset", metadata);
                var data = BuildEngineFactory.DefaultContent;

                BuildManifest manifest;
                var facade = BuildEngineFactory.CreateWithPreviousManifest(asset, data, out manifest);

                manifest.Items[0] = BuildManifestHelper.CloneWithoutMetadata(manifest.Items[0]);

                facade.FileSystem.GetFile("/input/assets/simple.asset").Create(BuildEngineFactory.DefaultContent);
                facade.FileSystem.GetCreatedFile("/output/assets/simple.dat");

                facade.Configuration.Incremental = true;
                facade.CreateBuildEngine();

                // When
                var result = facade.Engine.Build(facade.Configuration, manifest);

                // Then
                Assert.True(facade.Configuration.Incremental);
                Assert.Equal(AssetBuildStatus.Success, result.Items[0].Status);
                Assert.True(facade.Log.Messages.Contains("Asset metadata have changed. Rebuilding asset."));
            }

            [Fact]
            public void Should_Rebuild_Asset_If_Metadata_Keys_Have_Changed()
            {
                // Given
                var metadata = new Dictionary<string, string> {{"Key", "Value"}};
                var asset = new AssetDefinition("assets/simple.asset", metadata);
                var data = BuildEngineFactory.DefaultContent;

                BuildManifest manifest;
                var facade = BuildEngineFactory.CreateWithPreviousManifest(asset, data, out manifest);

                var newMetadata = new Dictionary<string, string> {{"Key2", "Value"}};
                manifest.Items[0] = BuildManifestHelper.CloneWithMetadata(manifest.Items[0], newMetadata);

                facade.FileSystem.GetFile("/input/assets/simple.asset").Create(BuildEngineFactory.DefaultContent);
                facade.FileSystem.GetCreatedFile("/output/assets/simple.dat");

                facade.Configuration.Incremental = true;
                facade.CreateBuildEngine();

                // When
                var result = facade.Engine.Build(facade.Configuration, manifest);

                // Then
                Assert.True(facade.Configuration.Incremental);
                Assert.Equal(AssetBuildStatus.Success, result.Items[0].Status);
                Assert.True(facade.Log.Messages.Contains("Asset metadata have changed. Rebuilding asset."));
            }

            [Fact]
            public void Should_Rebuild_Asset_If_Metadata_Values_Have_Changed()
            {
                // Given
                var metadata = new Dictionary<string, string> {{"Key", "Value"}};
                var asset = new AssetDefinition("assets/simple.asset", metadata);
                var data = BuildEngineFactory.DefaultContent;

                BuildManifest manifest;
                var facade = BuildEngineFactory.CreateWithPreviousManifest(asset, data, out manifest);

                var newMetadata = new Dictionary<string, string> {{"Key", "Value2"}};
                manifest.Items[0] = BuildManifestHelper.CloneWithMetadata(manifest.Items[0], newMetadata);

                facade.FileSystem.GetFile("/input/assets/simple.asset").Create(BuildEngineFactory.DefaultContent);
                facade.FileSystem.GetCreatedFile("/output/assets/simple.dat");

                facade.Configuration.Incremental = true;
                facade.CreateBuildEngine();

                // When
                var result = facade.Engine.Build(facade.Configuration, manifest);

                // Then
                Assert.True(facade.Configuration.Incremental);
                Assert.Equal(AssetBuildStatus.Success, result.Items[0].Status);
                Assert.True(facade.Log.Messages.Contains("Asset metadata have changed. Rebuilding asset."));
            }

            [Fact]
            public void Should_Rebuild_Asset_If_Dependency_Does_Not_Exist()
            {
                // Given
                var asset = new AssetDefinition("assets/simple.asset");
                var data = BuildEngineFactory.DefaultContent;

                BuildManifest manifest;
                var facade = BuildEngineFactory.CreateWithPreviousManifest(asset, data, out manifest);

                facade.FileSystem.GetFile("/input/assets/simple.asset").Create(BuildEngineFactory.DefaultContent);
                facade.FileSystem.GetCreatedFile("/output/assets/simple.dat");

                // Add file that does not exist in the file system.
                manifest.Items[0].Dependencies = new[] {new AssetDependency("assets/other.asset", 0, "ABC")};

                facade.Configuration.Incremental = true;
                facade.CreateBuildEngine();

                // When
                var result = facade.Engine.Build(facade.Configuration, manifest);

                // Then
                Assert.True(facade.Configuration.Incremental);
                Assert.Equal(AssetBuildStatus.Success, result.Items[0].Status);
                Assert.True(facade.Log.Messages.Contains("The dependency 'assets/other.asset' has been removed. Rebuilding asset."));
            }

            [Fact]
            public void Should_Rebuild_Asset_If_Dependency_File_Size_Has_Changed()
            {
                // Given
                var asset = new AssetDefinition("assets/simple.asset");
                var data = BuildEngineFactory.DefaultContent;

                BuildManifest manifest;
                var facade = BuildEngineFactory.CreateWithPreviousManifest(asset, data, out manifest);

                facade.FileSystem.GetFile("/input/assets/simple.asset").Create(BuildEngineFactory.DefaultContent);
                facade.FileSystem.GetCreatedFile("/output/assets/simple.dat");

                // Add file that does not exist in the file system.
                manifest.Items[0].Dependencies = new[] {new AssetDependency("assets/other.asset", 2, "ABC")};
                var file = facade.FileSystem.GetCreatedFile("/input/assets/other.asset");
                file.Create(new byte[] {1, 2, 3});

                facade.Configuration.Incremental = true;
                facade.CreateBuildEngine();

                // When
                var result = facade.Engine.Build(facade.Configuration, manifest);

                // Then
                Assert.True(facade.Configuration.Incremental);
                Assert.Equal(AssetBuildStatus.Success, result.Items[0].Status);
                Assert.True(facade.Log.Messages.Contains("The file size of dependency 'assets/other.asset' has changed. Rebuilding asset."));
            }

            [Fact]
            public void Should_Rebuild_Asset_If_Dependency_Checksum_Has_Changed()
            {
                // Given
                var asset = new AssetDefinition("assets/simple.asset");
                var data = BuildEngineFactory.DefaultContent;

                BuildManifest manifest;
                var facade = BuildEngineFactory.CreateWithPreviousManifest(asset, data, out manifest);

                facade.FileSystem.GetFile("/input/assets/simple.asset").Create(BuildEngineFactory.DefaultContent);
                facade.FileSystem.GetCreatedFile("/output/assets/simple.dat");

                // Add file that does not exist in the file system.
                manifest.Items[0].Dependencies = new[] {new AssetDependency("assets/other.asset", 3, "ABC")};
                var file = facade.FileSystem.GetCreatedFile("/input/assets/other.asset");
                file.Create(new byte[] {1, 2, 3});

                facade.Configuration.Incremental = true;
                facade.CreateBuildEngine();

                // When
                var result = facade.Engine.Build(facade.Configuration, manifest);

                // Then
                Assert.True(facade.Configuration.Incremental);
                Assert.Equal(AssetBuildStatus.Success, result.Items[0].Status);
                Assert.True(facade.Log.Messages.Contains("The checksum of dependency 'assets/other.asset' has changed. Rebuilding asset."));
            }

            [Fact]
            public void Should_Not_Rebuild_Asset_If_Changed_If_Incremental_Build_Has_Been_Turned_Off()
            {
                // Given
                var asset = new AssetDefinition("assets/simple.asset");
                var data = BuildEngineFactory.DefaultContent;

                BuildManifest manifest;
                var facade = BuildEngineFactory.CreateWithPreviousManifest(asset, data, out manifest);

                manifest.Items[0].Length = data.Length - 1;

                facade.FileSystem.GetFile("input/assets/simple.asset").Create(BuildEngineFactory.DefaultContent);
                facade.FileSystem.GetCreatedFile("output/assets/simple.dat");

                facade.Configuration.Incremental = false;
                facade.CreateBuildEngine();

                // When
                var result = facade.Engine.Build(facade.Configuration, manifest);

                // Then
                Assert.Equal(AssetBuildStatus.Success, result.Items[0].Status);
                Assert.True(facade.Log.Messages.Contains("Built @assets/simple.asset"));
            }

            [Fact]
            public void Should_Throw_Exception_If_Building_Content_With_A_Disposed_Build_Engine()
            {
                // Given
                var facade = new BuildEngineFactory();
                facade.CreateBuildEngine();
                facade.Engine.Dispose();

                // When
                var result = Record.Exception(() => facade.Engine.Build(facade.Configuration));

                // Then
                Assert.IsType<ObjectDisposedException>(result);
                Assert.True(result.Message.StartsWith("The build engine has been disposed."));
            }

            [Fact]
            public void Should_Expand_Globbed_Assets()
            {
                // Given
                var fileSystem = new GlobberFixture().FileSystem;
                var facade = new BuildEngineFactory(fileSystem);
                var engine = facade.CreateBuildEngine();

                var configuration = new BuildConfiguration();
                configuration.InputDirectory = "/Temp";
                configuration.OutputDirectory = "/Output";
                configuration.Assets.Add(new AssetDefinition("**/*.txt"));

                // When
                var result = engine.Build(configuration);

                // Then
                Assert.Equal(2, result.Items.Count);
                Assert.Equal("Hello/World/Text.txt", result.Items[0].Asset.Path.FullPath);
                Assert.Equal("Goodbye/OtherText.txt", result.Items[1].Asset.Path.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Expanded_Globbed_Asset_Path_Is_Not_Relative_To_Input_Directory()
            {
                // Given
                var fileSystem = new GlobberFixture().FileSystem;
                fileSystem.GetCreatedDirectory("/Input");

                var facade = new BuildEngineFactory(fileSystem);
                var engine = facade.CreateBuildEngine();

                var configuration = new BuildConfiguration();
                configuration.InputDirectory = "/Input";
                configuration.OutputDirectory = "/Output";
                configuration.Assets.Add(new AssetDefinition("/Temp/**/*.txt"));

                // When
                var result = Record.Exception(() => engine.Build(configuration));

                // Then
                Assert.IsType<LuntException>(result);
                Assert.Equal("Invalid glob pattern. Expected pattern '/Temp/**/*.txt' to be relative to input directory '/Input'.", result.Message);
            }
        }
    }
}