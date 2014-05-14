namespace Lunt.Bootstrapping
{
    /// <summary>
    /// Represents a bootstrapper.
    /// </summary>
    public interface IBootstrapper
    {
        /// <summary>
        /// Initializes the bootstrapper.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Gets the build kernel.
        /// </summary>
        /// <returns>The build kernel.</returns>
        IBuildKernel GetKernel();
    }
}