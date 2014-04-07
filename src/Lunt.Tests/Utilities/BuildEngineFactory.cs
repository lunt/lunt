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
using Lunt.IO;
using Lunt.Tests.Extensions;
using Lunt.Tests.Fakes;

namespace Lunt.Tests.Utilities
{
    public class BuildEngineFactory
    {
        private BuildEngine _engine;
        private readonly FakeFileSystem _fileSystem;
        private readonly FakeBuildEnvironment _environment;
        private readonly IHashComputer _hasher;
        private readonly BuildConfiguration _configuration;
        private readonly FakeComponentCollection _components;
        private readonly FakeBuildLog _log;

        public static byte[] DefaultContent = {0, 1, 2, 3, 4, 5};
        public static string DefaultHash = "A_DUMMY_HASH";

        public BuildEngine Engine
        {
            get { return _engine; }
        }

        public FakeFileSystem FileSystem
        {
            get { return _fileSystem; }
        }

        public FakeComponentCollection Components
        {
            get { return _components; }
        }

        public FakeBuildLog Log
        {
            get { return _log; }
        }

        public IHashComputer Hasher
        {
            get { return _hasher; }
        }

        public BuildConfiguration Configuration
        {
            get { return _configuration; }
        }

        public BuildEngineFactory(FakeFileSystem fileSystem = null)
        {
            _fileSystem = fileSystem ?? new FakeFileSystem();

            _configuration = new BuildConfiguration();
            _configuration.InputDirectory = "/input";
            _configuration.OutputDirectory = "/output";

            // Create the input directory.
            _fileSystem.GetDirectory(_configuration.InputDirectory).Create();

            _environment = new FakeBuildEnvironment(_fileSystem);
            _components = new FakeComponentCollection();
            _log = new FakeBuildLog();
            _hasher = new FakeHashComputer(DefaultHash);
        }

        public BuildEngine CreateBuildEngine()
        {
            _engine = new BuildEngine(_environment, _components, Hasher, _log);
            return _engine;
        }

        public static BuildEngineFactory CreateWithPreviousManifest(AssetDefinition assetDefinition, byte[] data, out BuildManifest manifest)
        {
            var facade = new BuildEngineFactory();

            var sourceFilePath = facade.Configuration.InputDirectory.Combine(assetDefinition.Path);
            var sourceFile = facade.FileSystem.GetFile(sourceFilePath);
            if (data != null)
            {
                sourceFile.Create(data);
            }

            var processor = new FakeProcessor<string, string>((c, v) => v);
            var importer = FakeImporter<string>.Mock((c, f) => string.Empty, ".asset", processor.GetType());
            var writer = FakeWriter<string>.Mock((c, f, v) => { });

            facade.Configuration.Assets.Add(assetDefinition);
            facade.Components.Importers.Add(importer);
            facade.Components.Processors.Add(processor);
            facade.Components.Writers.Add(writer);

            var asset = new Asset(assetDefinition.Path);

            manifest = new BuildManifest();
            manifest.Items.Add(new BuildManifestItem(asset)
            {
                Length = data != null ? data.Length : 0,
                Message = string.Empty,
                Status = AssetBuildStatus.Success,
                Checksum = data != null ? facade.Hasher.Compute(sourceFile) : string.Empty
            });

            return facade;
        }
    }
}