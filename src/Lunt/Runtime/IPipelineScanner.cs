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
        /// <returns></returns>
        IEnumerable<IPipelineComponent> Scan();
    }
}