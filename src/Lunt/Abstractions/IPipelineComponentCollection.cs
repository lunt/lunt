using System.Collections.Generic;

namespace Lunt
{
    /// <summary>
    /// Provides a mechanism for enumerating pipeline components.
    /// </summary>
    public interface IPipelineComponentCollection : IEnumerable<IPipelineComponent>
    {
    }
}