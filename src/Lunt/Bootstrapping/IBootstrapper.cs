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
        /// Gets the build engine.
        /// </summary>
        /// <returns>The build engine.</returns>
        IBuildEngine GetEngine();
    }
}