using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lunt.Diagnostics;
using Lunt.IO;

namespace Lunt.Runtime
{
    /// <summary>
    /// Provides a mechanism for finding pipeline components within a directory.
    /// </summary>
    public sealed class DirectoryScanner : IPipelineScanner
    {
        private readonly IBuildLog _log;
        private readonly List<Assembly> _ignoredAssemblies;
        private readonly AssemblyTypeScanner _typeScanner;
        private readonly IDirectory _directory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryScanner" /> class.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="directory">The directory.</param>
        public DirectoryScanner(IBuildLog log, IDirectory directory)
        {
            _typeScanner = new AssemblyTypeScanner(log);
            _log = log;
            _directory = directory;

            _ignoredAssemblies = new List<Assembly>();
            _ignoredAssemblies.Add(typeof (BuildEngine).Assembly);
        }

        /// <summary>
        /// Performs a scan for components in the specified directory.
        /// </summary>
        /// <returns>The components that was found during scan.</returns>
        public IEnumerable<IPipelineComponent> Scan()
        {
            // Make sure that the component directory exist.
            if (!_directory.Exists)
            {
                _log.Error("Directory {0} does not exist.", _directory.Path);
                return Enumerable.Empty<IPipelineComponent>();
            }

            var result = new List<IPipelineComponent>();

            // Get all the assemblies in the directory.
            var files = _directory.GetFiles("*.dll", SearchScope.Current);
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
                var components = _typeScanner.Scan<IPipelineComponent>(assembly);
                foreach (var component in components)
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