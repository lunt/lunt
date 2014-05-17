namespace Lunt
{
    /// <summary>
    /// Represents a bootstrapper.
    /// </summary>
    public interface IBootstrapper
    {
        /// <summary>
        /// Resolves a service.
        /// </summary>
        /// <typeparam name="T">The service type to resolve.</typeparam>
        /// <returns>The resolved service.</returns>
        T GetService<T>() where T : class;
    }
}