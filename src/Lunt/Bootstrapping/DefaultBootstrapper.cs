using Lunt.Bootstrapping.TinyIoc;
using Lunt.Diagnostics;
using Lunt.IO;
using Lunt.Runtime;

namespace Lunt.Bootstrapping
{
    /// <summary>
    /// The default bootstrapper that uses TinyIoc.
    /// </summary>
    public class DefaultBootstrapper : Bootstrapper<TinyIoCContainer>
    {
        private bool _disposed;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (ApplicationContainer != null)
                    {
                        ApplicationContainer.Dispose();
                        ApplicationContainer = null;
                    }
                }
                _disposed = true;
            }
        }

        /// <summary>
        /// Creates an TinyIoC container.
        /// </summary>
        /// <returns>A TinyIoC container.</returns>
        protected override TinyIoCContainer CreateContainer()
        {
            return new TinyIoCContainer();
        }

        /// <summary>
        /// Resolves the build engine.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>The resolved build engine.</returns>
        protected override IBuildEngine ResolveBuildEngine(TinyIoCContainer container)
        {
            return container.Resolve<IBuildEngine>();
        }

        /// <summary>
        /// Registers the build engine.
        /// </summary>
        /// <param name="container">The container.</param>
        protected override sealed void RegisterBuildEngine(TinyIoCContainer container)
        {
            container.Register(typeof (IBuildEngine), typeof (BuildEngine));
        }

        /// <summary>
        /// Registers the build environment.
        /// </summary>
        /// <param name="container">The container.</param>
        protected override void RegisterBuildEnvironment(TinyIoCContainer container)
        {
            container.Register(typeof(IBuildEnvironment), typeof(BuildEnvironment)).AsSingleton();
        }

        /// <summary>
        /// Registers the file system.
        /// </summary>
        /// <param name="container">The container.</param>
        protected override void RegisterFileSystem(TinyIoCContainer container)
        {
            container.Register(typeof(IFileSystem), typeof(FileSystem)).AsSingleton();
        }

        /// <summary>
        /// Registers the hash computer.
        /// </summary>
        /// <param name="container">The container.</param>
        protected override void RegisterHashComputer(TinyIoCContainer container)
        {
            container.Register(typeof(IHashComputer), typeof(HashComputer)).AsSingleton();
        }

        /// <summary>
        /// Registers the build log.
        /// </summary>
        /// <param name="container">The container.</param>
        protected override void RegisterBuildLog(TinyIoCContainer container)
        {
            container.Register(typeof(IBuildLog), typeof(TraceBuildLog)).AsSingleton();
        }

        /// <summary>
        /// Registers the pipeline scanner.
        /// </summary>
        /// <param name="container">The container.</param>
        protected override void RegisterPipelineScanner(TinyIoCContainer container)
        {
            container.Register(typeof(IPipelineScanner), typeof(AppDomainScanner)).AsSingleton();
        }
    }
}