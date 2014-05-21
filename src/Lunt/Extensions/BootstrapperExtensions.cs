using Lunt.Diagnostics;
using Lunt.IO;
using Lunt.Runtime;

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
        public static IBuildKernel GetBuildKernel(this IBootstrapper bootstrapper)
        {
            return bootstrapper.GetService<IBuildKernel>();
        }

        /// <summary>
        /// Gets the file system.
        /// </summary>
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <returns>A file system.</returns>
        public static IFileSystem GetFileSystem(this IBootstrapper bootstrapper)
        {
            return bootstrapper.GetService<IFileSystem>();
        }

        /// <summary>
        /// Gets the build log.
        /// </summary>
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <returns>A build log.</returns>
        public static IBuildLog GetBuildLog(this IBootstrapper bootstrapper)
        {
            return bootstrapper.GetService<IBuildLog>();
        }

        /// <summary>
        /// Gets the hash computer.
        /// </summary>
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <returns>A hash computer.</returns>
        public static IHashComputer GetHashComputer(this IBootstrapper bootstrapper)
        {
            return bootstrapper.GetService<IHashComputer>();
        }

        /// <summary>
        /// Gets the pipeline scanner.
        /// </summary>
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <returns>A pipeline scanner.</returns>
        public static IPipelineScanner GetPipelineScanner(this IBootstrapper bootstrapper)
        {
            return bootstrapper.GetService<IPipelineScanner>();
        }

        /// <summary>
        /// Gets the build manifest provider.
        /// </summary>
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <returns>A build manifest provider.</returns>
        public static IBuildManifestProvider GetBuildManifestProvider(this IBootstrapper bootstrapper)
        {
            return bootstrapper.GetService<IBuildManifestProvider>();
        }
    }
}
