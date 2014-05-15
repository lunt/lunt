namespace Lunt
{
    /// <summary>
    /// Represents a bootstrapper.
    /// </summary>
    public interface IBootstrapper
    {
        /// <summary>
        /// Gets the build kernel.
        /// </summary>
        /// <returns>The build kernel.</returns>
        IBuildKernel GetKernel();
    }
}