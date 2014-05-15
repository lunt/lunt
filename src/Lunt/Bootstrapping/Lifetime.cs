namespace Lunt.Bootstrapping
{
    /// <summary>
    /// Represents a lifetime.
    /// </summary>
    public enum Lifetime
    {
        /// <summary>
        /// The default lifetime.
        /// </summary>
        Default,
        /// <summary>
        /// Singleton lifetime.
        /// </summary>
        Singleton,
        /// <summary>
        /// The instance per dependency lifetime.
        /// </summary>
        InstancePerDependency
    }
}
