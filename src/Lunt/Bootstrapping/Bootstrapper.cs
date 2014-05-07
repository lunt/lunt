using System;

namespace Lunt.Bootstrapping
{
    /// <summary>
    /// The bootstrapper base class.
    /// </summary>
    /// <typeparam name="TContainer">The container type.</typeparam>
    public abstract class Bootstrapper<TContainer> : IBootstrapper, IDisposable
        where TContainer : class 
    {
        /// <summary>
        /// Gets or sets the application container.
        /// </summary>
        /// <value>The application container.</value>
        public TContainer ApplicationContainer { get; protected set; }

        public void Dispose()
        {            
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {            
        }

        /// <summary>
        /// Initializes the bootstrapper.
        /// </summary>
        public void Initialize()
        {
            ApplicationContainer = CreateContainer();

            // Register stuff.
            RegisterBuildEngine(ApplicationContainer);
            RegisterBuildEnvironment(ApplicationContainer);
            RegisterFileSystem(ApplicationContainer);
            RegisterHashComputer(ApplicationContainer);
            RegisterBuildLog(ApplicationContainer);
            RegisterPipelineScanner(ApplicationContainer);

            ConfigureContainer(ApplicationContainer);
        }

        /// <summary>
        /// Gets the build engine.
        /// </summary>
        /// <returns>The build engine.</returns>
        public IBuildEngine GetEngine()
        {
            if (ApplicationContainer == null)
            {
                throw new LuntException("Bootstrapper have not been initialized.");
            }

            return ResolveBuildEngine(ApplicationContainer);
        }

        /// <summary>
        /// Allows the container to be configured.
        /// </summary>
        /// <param name="container">The container to be configured.</param>
        protected virtual void ConfigureContainer(TContainer container)
        {
        }

        /// <summary>
        /// Creates the container.
        /// </summary>
        /// <returns>A container.</returns>
        protected abstract TContainer CreateContainer();

        /// <summary>
        /// Resolves the build engine.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>The build engine.</returns>
        protected abstract IBuildEngine ResolveBuildEngine(TContainer container);

        /// <summary>
        /// Registers the build engine.
        /// </summary>
        /// <param name="container">The container.</param>
        protected abstract void RegisterBuildEngine(TContainer container);

        /// <summary>
        /// Registers the build environment.
        /// </summary>
        /// <param name="container">The container.</param>
        protected abstract void RegisterBuildEnvironment(TContainer container);

        /// <summary>
        /// Registers the file system.
        /// </summary>
        /// <param name="container">The container.</param>
        protected abstract void RegisterFileSystem(TContainer container);

        /// <summary>
        /// Registers the hash computer.
        /// </summary>
        /// <param name="container">The container.</param>
        protected abstract void RegisterHashComputer(TContainer container);

        /// <summary>
        /// Registers the build log.
        /// </summary>
        /// <param name="container">The container.</param>
        protected abstract void RegisterBuildLog(TContainer container);

        /// <summary>
        /// Registers the pipeline scanner.
        /// </summary>
        /// <param name="container">The container.</param>
        protected abstract void RegisterPipelineScanner(TContainer container);
    }
}
