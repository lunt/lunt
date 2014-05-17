namespace Lunt.Extensions
{
    /// <summary>
    /// Bootstrapper convenience extensions.
    /// </summary>
    public static class BootstrapperExtensions
    {
        /// <summary>
        /// Gets the build environment.
        /// </summary>
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <returns>A build environment.</returns>
        public static IBuildEnvironment GetBuildEnvironment(this IBootstrapper bootstrapper)
        {
            return bootstrapper.GetService<IBuildEnvironment>();
        }

        /// <summary>
        /// Gets the build configuration reader.
        /// </summary>
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <returns>A build configuration reader.</returns>
        public static IBuildConfigurationReader GetBuildConfigurationReader(this IBootstrapper bootstrapper)
        {
            return bootstrapper.GetService<IBuildConfigurationReader>();
        }

        /// <summary>
        /// Gets the build kernel.
        /// </summary>
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <returns>A build kernel.</returns>
        public static IBuildKernel GetKernel(this IBootstrapper bootstrapper)
        {
            return bootstrapper.GetService<IBuildKernel>();
        }
    }
}
