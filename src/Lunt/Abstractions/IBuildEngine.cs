namespace Lunt
{
    /// <summary>
    /// The Lunt build kernel.
    /// </summary>
    public interface IBuildEngine
    {
        /// <summary>
        /// Performs a build using the specified build configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The build manifest.</returns>
        BuildManifest Build(BuildConfiguration configuration);

        /// <summary>
        /// Performs a build using the specified build configuration and manifest.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="previousManifest">The previous manifest.</param>
        /// <returns>The build manifest.</returns>
        BuildManifest Build(BuildConfiguration configuration, BuildManifest previousManifest);
    }
}