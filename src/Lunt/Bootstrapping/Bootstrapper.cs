using System.Collections.Generic;
using System.Linq;

namespace Lunt.Bootstrapping
{
    /// <summary>
    /// The bootstrapper base class.
    /// </summary>
    public abstract class Bootstrapper<TContainer> : IBootstrapper
        where TContainer : class 
    {
        private TContainer _container;
        private readonly object _lock;
        private readonly IInternalConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bootstrapper{TContainer}"/> class.
        /// </summary>
        /// <param name="configuration">The internal configuration to be used.</param>
        protected Bootstrapper(IInternalConfiguration configuration = null)
        {
            _lock = new object();
            _configuration = configuration ?? new InternalConfiguration();
        }

        /// <summary>
        /// Gets the build kernel.
        /// </summary>
        /// <returns>The build kernel.</returns>
        public IBuildKernel GetKernel()
        {
            lock (_lock)
            {
                if (_container == null)
                {
                    // Create a new container.
                    _container = CreateContainer();
                   
                    // Get all registrations and add the build kernel registration.
                    var registrations = GetRegistrations().ToList();
                    registrations.Add(new TypeRegistration(typeof(IBuildKernel), typeof(BuildKernel), Lifetime.Singleton));

                    // Perform registrations.
                    RegisterTypeRegistrations(_container, registrations.OfType<TypeRegistration>());
                    RegisterInstanceRegistrations(_container, registrations.OfType<InstanceRegistration>());
                    RegisterFactoryRegistrations(_container, registrations.OfType<FactoryRegistration>());
                    
                    // Let the bootstrapper configure the container as well.
                    // Good for more advanced tinkering.
                    ConfigureContainer(_container);
                }
                return ResolveKernel(_container);
            }
        }

        /// <summary>
        /// Gets all registrations from the internal configuration.
        /// </summary>
        /// <returns>All registrations.</returns>
        protected virtual IEnumerable<ContainerRegistration> GetRegistrations()
        {
            return _configuration.GetRegistrations();
        }

        /// <summary>
        /// Creates the container.
        /// </summary>
        /// <returns>A new container.</returns>
        protected abstract TContainer CreateContainer();

        /// <summary>
        /// Configures the container.
        /// </summary>
        protected virtual void ConfigureContainer(TContainer container)
        {            
        }

        /// <summary>
        /// Resolves the kernel.
        /// </summary>
        /// <param name="container">The container to resolve the kernel from.</param>
        /// <returns>The build kernel.</returns>
        protected abstract IBuildKernel ResolveKernel(TContainer container);

        /// <summary>
        /// Registers type registrations.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="registrations">The registrations to register with the container.</param>
        protected abstract void RegisterTypeRegistrations(TContainer container, IEnumerable<TypeRegistration> registrations);

        /// <summary>
        /// Registers instance registrations.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="registrations">The registrations to register with the container.</param>
        protected abstract void RegisterInstanceRegistrations(TContainer container, IEnumerable<InstanceRegistration> registrations);

        /// <summary>
        /// Registers factory registrations.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="registrations">The registrations to register with the container.</param>
        protected abstract void RegisterFactoryRegistrations(TContainer container, IEnumerable<FactoryRegistration> registrations);
    }
}
