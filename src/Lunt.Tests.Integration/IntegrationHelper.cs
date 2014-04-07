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
using System.Collections.Generic;
using Lunt.Arguments;
using Lunt.Commands;
using Lunt.IO;
using Lunt.Tests.Integration.Pipeline;
using Lunt.Tests.Integration.Utilities;
using IOFile = System.IO.File;
using IOPath = System.IO.Path;

namespace Lunt.Tests.Integration
{
    using Lunt.Arguments;
    using Lunt.Commands;
    using Lunt.IO;
    using Lunt.Tests.Integration.Pipeline;
    using Lunt.Tests.Integration.Utilities;

    public static class IntegrationHelper
    {
        public static LuntApplication CreateApplication()
        {
            var console = new ConsoleWriter();
            var log = new TraceBuildLog();
            var parser = new ArgumentParser(log);
            var scannerFactory = new PipelineScannerFactory(log);
            var factory = new CommandFactory(log, console, null, scannerFactory);
            return new LuntApplication(console, log, parser, factory);
        }

        public static BuildManifest GetBuildManifest(IntegrationContext context, string[] args)
        {
            var configurationFile = IOPath.GetFileName(args[args.Length - 1]) + ".manifest";
            var manifestPath = context.GetTargetPath(configurationFile);
            if (!IOFile.Exists(manifestPath))
            {
                return null;
            }
            var reader = new BuildManifestProvider();
            return reader.LoadManifest(new FileSystem(), new FilePath(manifestPath));
        }

        public static LuntOptions CreateOptions(IntegrationContext context)
        {
            return CreateOptions(context, null);
        }

        public static LuntOptions CreateOptions(IntegrationContext context, string configurationPath)
        {
            var options = new LuntOptions();
            options.InputDirectory = context.AssetsPath;
            options.OutputDirectory = context.OutputPath;
            if (configurationPath != null)
            {
                options.BuildConfiguration = context.GetTargetPath(configurationPath);
            }
            return options;
        }

        public static string[] CreateArgs(IntegrationContext context, LuntOptions options)
        {
            var args = new List<string>();
            if (options.InputDirectory != null)
            {
                args.Add(string.Concat("-i=\"", options.InputDirectory.FullPath, "\""));
            }
            if (options.OutputDirectory != null)
            {
                args.Add(string.Concat("-o=\"", options.OutputDirectory.FullPath, "\""));
            }
            if (options.OutputDirectory != null)
            {
                args.Add("-v=d");
            }
            if (options.BuildConfiguration != null)
            {
                args.Add(options.BuildConfiguration.FullPath);
            }
            return args.ToArray();
        }
    }
}
