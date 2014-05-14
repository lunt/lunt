using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lunt.Diagnostics;
using Lunt.IO;

namespace Lunt.Runtime
{
    /// <summary>
    /// Provides a mechanism for finding pipeline components within the working directory.
    /// </summary>
    public sealed class WorkingDirectoryScanner : IPipelineScanner
    {   
        private readonly IBuildLog _log;
        private readonly IBuildEnvironment _environment;
        private readonly List<Assembly> _ignoredAssemblies;
        private readonly AssemblyTypeScanner _typeScanner;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkingDirectoryScanner"/> class.
        /// </summary>
        /// <param name="log">The build log.</param>
        /// <param name="environment">The build environment.</param>
        public WorkingDirectoryScanner(IBuildLog log, IBuildEnvironment environment)
        {
            _typeScanner = new AssemblyTypeScanner(log);
            _log = log;
            _environment = environment;

            _ignoredAssemblies = new List<Assembly>();
            _ignoredAssemblies.Add(typeof (BuildKernel).Assembly);
        }

        /// <summary>
        /// Performs a scan for components.
        /// </summary>
        /// <returns>The components that was found during scan.</returns>
        public IEnumerable<IPipelineComponent> Scan()
        {
            var directoryPath = _environment.GetWorkingDirectory();
            var directory = _environment.FileSystem.GetDirectory(directoryPath);

            // Make sure that the component directory exist.
            if (!directory.Exists)
            {
                _log.Error("Directory {0} does not exist.", directory.Path);
                return Enumerable.Empty<IPipelineComponent>();
            }

            var result = new List<IPipelineComponent>();

            // Get all the assemblies in the directory.
            var files = directory.GetFiles("*.dll", SearchScope.Current);
            foreach (var file in files)
            {
                _log.Debug("Examining assembly {0}...", file.Path.GetFilename());

                // Load the assembly into the temporary application domain.
                var assembly = LoadAssembly(file);
                if (assembly == null)
                {
                    continue;
                }

                // Ignored?
                if (_ignoredAssemblies.Contains(assembly))
                {
                    continue;
                }

                // Got any types here?
                var types = _typeScanner.Scan<IPipelineComponent>(assembly);
                foreach (var component in types)
                {
                    result.Add(component);
                }
            }

            return result;
        }

        private Assembly LoadAssembly(IFile file)
        {
            try
            {
                var name = AssemblyName.GetAssemblyName(file.Path.FullPath);
                return AppDomain.CurrentDomain.Load(name);
            }
            catch (BadImageFormatException)
            {
                _log.Error(Verbosity.Diagnostic, "Could not load assembly '{0}' (Bad image format).", file.Path.GetFilename());
                return null;
            }
        }
    }
}
