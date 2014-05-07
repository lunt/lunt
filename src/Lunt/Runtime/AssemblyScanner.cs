using System.Collections.Generic;
using System.Reflection;
using Lunt.Diagnostics;

namespace Lunt.Runtime
{
    /// <summary>
    /// Provides a mechanism for finding pipeline components within an assembly.
    /// </summary>
    public sealed class AssemblyScanner : IPipelineScanner
    {
        private readonly AssemblyTypeScanner _typeScanner;
        private readonly Assembly _assembly;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyScanner" /> class.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="assembly">The assembly.</param>
        public AssemblyScanner(IBuildLog log, Assembly assembly)
        {
            _typeScanner = new AssemblyTypeScanner(log);
            _assembly = assembly;
        }

        /// <summary>
        /// Performs a scan for components.
        /// </summary>
        /// <returns>The components that was found during scan.</returns>
        public IEnumerable<IPipelineComponent> Scan()
        {
            return _typeScanner.Scan<IPipelineComponent>(_assembly, log: true);
        }
    }
}