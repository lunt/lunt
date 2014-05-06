using System.Collections.Generic;
using System.Reflection;
using Lunt.Diagnostics;

namespace Lunt.Runtime
{
    /// <summary>
    /// Provides a mechanism for finding pipeline components within an assembly.
    /// </summary>
    public sealed class PipelineAssemblyScanner : IPipelineScanner
    {
        private readonly AssemblyScanner _scanner;
        private readonly Assembly _assembly;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineAssemblyScanner" /> class.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="assembly">The assembly.</param>
        public PipelineAssemblyScanner(IBuildLog log, Assembly assembly)
        {
            _scanner = new AssemblyScanner(log);
            _assembly = assembly;
        }

        /// <summary>
        /// Performs a scan for components.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IPipelineComponent> Scan()
        {
            return _scanner.Scan<IPipelineComponent>(_assembly, log: true);
        }
    }
}