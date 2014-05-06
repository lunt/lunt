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
    public sealed class PipelineDirectoryScanner : IPipelineScanner, IDisposable
    {
        private readonly AppDomain _domain;
        private readonly IBuildLog _log;
        private readonly List<Assembly> _ignoredAssemblies;
        private readonly AssemblyScanner _scanner;
        private readonly IDirectory _directory;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineDirectoryScanner" /> class.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="directory">The directory.</param>
        public PipelineDirectoryScanner(IBuildLog log, IDirectory directory)
        {
            _domain = AppDomain.CreateDomain("Lunt-Temporary");
            _scanner = new AssemblyScanner(log);
            _log = log;
            _directory = directory;

            _ignoredAssemblies = new List<Assembly>();
            _ignoredAssemblies.Add(typeof (IPipelineComponent).Assembly);
            _ignoredAssemblies.Add(typeof (BuildEngine).Assembly);
            _ignoredAssemblies.Add(typeof (PipelineDirectoryScanner).Assembly);
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="PipelineDirectoryScanner"/> class.
        /// </summary>
        public void Dispose()
        {
            if (_domain != null)
            {
                AppDomain.Unload(_domain);
            }
        }

        /// <summary>
        /// Performs a scan for components in the specified directory.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IPipelineComponent> Scan()
        {
            // Make sure that the component directory exist.
            if (!_directory.Exists)
            {
                _log.Error("Directory {0} does not exist.", _directory.Path);
                return Enumerable.Empty<IPipelineComponent>();
            }

            // Find all assemblies that contains components.
            var assemblyFiles = new List<IFile>();

            // Get all the assemblies in the directory.
            IEnumerable<IFile> files = _directory.GetFiles("*.dll", SearchScope.Current);
            foreach (IFile file in files)
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
                var types = _scanner.Scan<IPipelineComponent>(assembly);
                if (types.Any())
                {
                    assemblyFiles.Add(file);
                }
            }

            var result = new List<IPipelineComponent>();

            // Load assemblies that contains the expected type into current domain.
            foreach (IFile assemblyFile in assemblyFiles)
            {
                _log.Verbose("Loading assembly {0}...", assemblyFile.Path.GetFilename());

                var assembly = LoadAssembly(assemblyFile);
                if (assembly == null)
                {
                    continue;
                }

                var components = _scanner.Scan<IPipelineComponent>(assembly, log: true);
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
                return _domain.Load(name);
            }
            catch (BadImageFormatException)
            {
                _log.Error(Verbosity.Diagnostic, "Could not load assembly '{0}' (Bad image format).", file.Path.GetFilename());
                return null;
            } 
        }
    }
}