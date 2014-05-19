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
        private bool _isInitialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bootstrapper{TContainer}"/> class.
        /// </summary>
        /// <param name="configuration">The internal configuration to be used.</param>
        protected Bootstrapper(IInternalConfiguration configuration = null)
        {
            _lock = new object();
            _configuration = configuration ?? new InternalConfiguration();
        }

        private void Initialize()
        {
            lock (_lock)
            {
                if (!_isInitialized)
                {
                    // Create a new container.
                    _container = CreateContainer();

                    // Get all registrations.
                    var registrations = _configuration.GetRegistrations().ToList();
                    InternalConfigurationValidator.Validate(registrations);

                    // Perform registrations.
                    RegisterTypeRegistrations(_container, registrations.OfType<TypeRegistration>());
                    RegisterInstanceRegistrations(_container, registrations.OfType<InstanceRegistration>());
                    RegisterFactoryRegistrations(_container, registrations.OfType<FactoryRegistration>());

                    // Let the bootstrapper configure the container as well.
                    // Good for more advanced tinkering.
                    ConfigureContainer(_container);

                    // We're now initialized.
                    _isInitialized = true;
                }
            }
        }

        /// <summary>
        /// Resolves a service.
        /// </summary>
        /// <typeparam name="T">The service type to resolve.</typeparam>
        /// <returns>The resolved service.</returns>
        public T GetService<T>()
            where T : class 
        {
            if (!_isInitialized)
            {
                Initialize();
            }
            return ResolveService<T>(_container);
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
        /// Resolves a service from the container.
        /// </summary>
        /// <typeparam name="T">The service type to resolve from the container.</typeparam>
        /// <param name="container">The container.</param>
        /// <returns>The resolved service.</returns>
        protected abstract T ResolveService<T>(TContainer container) where T : class;

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
