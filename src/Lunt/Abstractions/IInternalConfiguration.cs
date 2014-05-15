using System.Collections.Generic;
using Lunt.Bootstrapping;

namespace Lunt
{
    /// <summary>
    /// The internal configuration for Lunt that decides
    /// what internal components should be used.
    /// </summary>
    public interface IInternalConfiguration
    {
        /// <summary>
        /// Gets all registrations.
        /// </summary>
        /// <returns>All registrations.</returns>
        IEnumerable<ContainerRegistration> GetRegistrations();
    }
}