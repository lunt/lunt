using System.Collections.Generic;

namespace Lunt.Runtime
{
    /// <summary>
    /// Provides a mechanism for finding pipeline components.
    /// </summary>
    public interface IPipelineScanner
    {
        /// <summary>
        /// Performs a scan for components.
        /// </summary>
        /// <returns>The components that was found during scan.</returns>
        IEnumerable<IPipelineComponent> Scan();
    }
}