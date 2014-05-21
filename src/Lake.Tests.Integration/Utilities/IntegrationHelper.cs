using System.Collections.Generic;
using Lake.Arguments;
using Lake.Commands;
using Lunt;
using Lunt.IO;
using Lunt.Testing;

namespace Lake.Tests.Integration
{
    using System.IO;

    public static class IntegrationHelper
    {
        public static LakeApplication CreateApplication()
        {
            var console = new ConsoleWriter();
            var log = new TraceBuildLog();
            var parser = new ArgumentParser(log);
            var fileSystem = new FileSystem();
            var environment = new BuildEnvironment(fileSystem);
            var scannerFactory = new PipelineScannerFactory(log);
            var factory = new CommandFactory(log, console, environment, scannerFactory);
            return new LakeApplication(console, log, parser, factory);
        }

        public static BuildManifest GetBuildManifest(IntegrationContext context, string[] args)
        {
            var configurationFile = Path.GetFileName(args[args.Length - 1]) + ".manifest";
            var manifestPath = context.GetTargetPath(configurationFile);
            if (!File.Exists(manifestPath))
            {
                return null;
            }
            var reader = new BuildManifestProvider(new FileSystem());
            return reader.LoadManifest(new FilePath(manifestPath));
        }

        public static LakeOptions CreateOptions(IntegrationContext context, string configurationPath = null)
        {
            var options = new LakeOptions();
            options.InputDirectory = context.AssetsPath;
            options.OutputDirectory = context.OutputPath;
            if (configurationPath != null)
            {
                options.BuildConfiguration = context.GetTargetPath(configurationPath);
            }
            return options;
        }

        public static string[] CreateArgs(IntegrationContext context, LakeOptions options)
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
