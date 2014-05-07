using Lake.Arguments;
using Lake.Commands;
using Lake.Diagnostics;
using Lake.Runtime;
using Lunt;
using Lunt.IO;

namespace Lake
{
    public class Program
    {
        public static int Main(string[] args)
        {
            // Create the dependencies the Lake application need.
            var console = new ConsoleWriter();
            var log = new ConsoleBuildLog(console);
            var parser = new ArgumentParser(log);
            var fileSystem = new FileSystem();
            var environment = new BuildEnvironment(fileSystem);
            var scannerFactory = new PipelineScannerFactory(environment, log);
            var factory = new CommandFactory(log, console, environment, scannerFactory);

            // Create the application and run it.
            var application = new LakeApplication(console, log, parser, factory);
            return application.Run(args);
        }
    }
}