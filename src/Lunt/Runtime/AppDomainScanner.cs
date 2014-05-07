using System;
using System.Collections.Generic;
using Lunt.Diagnostics;

namespace Lunt.Runtime
{
    /// <summary>
    /// Provides a mechanism for finding pipeline components within the current application domain.
    /// </summary>
    public sealed class AppDomainScanner : IPipelineScanner
    {
        private readonly AssemblyTypeScanner _scanner;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppDomainScanner"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public AppDomainScanner(IBuildLog log)
        {
            _scanner = new AssemblyTypeScanner(log);
        }

        /// <summary>
        /// Performs a scan for components.
        /// </summary>
        /// <returns>The components that was found during scan.</returns>
        public IEnumerable<IPipelineComponent> Scan()
        {
            var result = new List<IPipelineComponent>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var components = _scanner.Scan<IPipelineComponent>(assembly);
                foreach (var component in components)
                {
                    result.Add(component);
                }
            }
            return result;
        }
    }
}
