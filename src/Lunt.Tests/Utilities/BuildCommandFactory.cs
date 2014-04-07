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
using Lunt.Commands;
using Lunt.Diagnostics;
using Lunt.IO;
using Lunt.Runtime;
using Lunt.Tests.Fakes;
using Moq;

namespace Lunt.Tests.Utilities
{
    internal class BuildCommandFactory
    {
        private readonly LuntOptions _options;
        private readonly BuildManifest _manifest;
        private readonly string _workingDirectory;
        private readonly BuildConfiguration _configuration;
        private readonly Mock<IBuildConfigurationReader> _configurationReader;
        private readonly Mock<IBuildEngine> _engine;
        private readonly Mock<IBuildEnvironment> _environment;
        private readonly Mock<IBuildManifestProvider> _manifestProvider;
        private readonly Mock<IPipelineScannerFactory> _scannerFactory;
        private readonly FakeConsole _console;

        public LuntOptions Options
        {
            get { return _options; }
        }

        public BuildManifest Manifest
        {
            get { return _manifest; }
        }

        public string WorkingDirectory
        {
            get { return _workingDirectory; }
        }

        public BuildConfiguration Configuration
        {
            get { return _configuration; }
        }

        public Mock<IBuildConfigurationReader> ConfigurationReader
        {
            get { return _configurationReader; }
        }

        public Mock<IBuildEngine> Engine
        {
            get { return _engine; }
        }

        public FakeConsole Console
        {
            get { return _console; }
        }

        public Mock<IPipelineScannerFactory> ScannerFactory
        {
            get { return _scannerFactory; }
        }

        public BuildCommandFactory()
        {
            _manifest = new BuildManifest();
            _options = new LuntOptions();
            _options.BuildConfiguration = "/build.config";
            _options.InputDirectory = "/input";
            _options.OutputDirectory = "/output";
            _workingDirectory = "/working";
            _configuration = new BuildConfiguration();
            _configurationReader = new Mock<IBuildConfigurationReader>();
            _engine = new Mock<IBuildEngine>();
            _environment = new Mock<IBuildEnvironment>();
            _manifestProvider = new Mock<IBuildManifestProvider>();
            _scannerFactory = new Mock<IPipelineScannerFactory>();
            _console = new FakeConsole();
        }

        public BuildCommand Create(IBuildConfigurationReader reader = null, 
            IBuildManifestProvider manifestProvider = null, 
            IPipelineScannerFactory factory = null, IBuildEngine engine = null)
        {
            var log = new Mock<IBuildLog>().Object;
            var hasher = new Mock<IHashComputer>().Object;

            if (factory == null)
            {
                ScannerFactory.Setup(x => x.Create(It.IsAny<DirectoryPath>()))
                    .Returns(new Mock<IPipelineScanner>().Object);
            }

            if (reader == null)
            {
                _configurationReader.Setup(x => x.Read(It.IsAny<FilePath>()))
                    .Returns(_configuration);
            }

            if (engine == null)
            {
                _engine.Setup(x => x.Build(It.IsAny<BuildConfiguration>(), It.IsAny<BuildManifest>()))
                    .Returns(_manifest);
            }

            _environment.Setup(x => x.GetWorkingDirectory())
                .Returns(() => _workingDirectory)
                .Verifiable();

            return new BuildCommand(_options, engine ?? _engine.Object, 
                log, _console, hasher, 
                factory ?? ScannerFactory.Object,
                _environment.Object, 
                manifestProvider ?? _manifestProvider.Object, 
                reader ?? _configurationReader.Object);
        }
    }
}
