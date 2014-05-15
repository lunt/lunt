using System.Collections.Generic;
using Lunt.Bootstrapping.TinyIoc;
using Lunt.Diagnostics;
using Lunt.IO;
using Lunt.Runtime;

namespace Lunt.Bootstrapping
{
    /// <summary>
    /// The default bootstrapper using TinyIoC.
    /// </summary>
    public class DefaultBootstrapper : Bootstrapper<TinyIoCContainer>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultBootstrapper"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public DefaultBootstrapper(IInternalConfiguration configuration = null)
            : base(configuration)
        {
        }

        /// <summary>
        /// Creates the container.
        /// </summary>
        /// <returns>A TinyIoC container.</returns>
        protected override TinyIoCContainer CreateContainer()
        {
            return new TinyIoCContainer();
        }

        /// <summary>
        /// Resolves the kernel.
        /// </summary>
        /// <param name="container">The container to resolve the kernel from.</param>
        /// <returns>The build kernel.</returns>
        protected sealed override IBuildKernel ResolveKernel(TinyIoCContainer container)
        {
            return container.Resolve<IBuildKernel>();
        }

        /// <summary>
        /// Registers type registrations.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="registrations">The registrations to register with the container.</param>
        protected sealed override void RegisterTypeRegistrations(TinyIoCContainer container, IEnumerable<TypeRegistration> registrations)
        {
            foreach (var registration in registrations)
            {
                var registered = container.Register(registration.RegistrationType, registration.ImplementationType);
                ConfigureLifetime(registered, registration.Lifetime);
            }
        }

        /// <summary>
        /// Registers instance registrations.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="registrations">The registrations to register with the container.</param>
        protected override sealed void RegisterInstanceRegistrations(TinyIoCContainer container, IEnumerable<InstanceRegistration> registrations)
        {
            foreach (var registration in registrations)
            {
                var registered = container.Register(registration.RegistrationType, registration.Instance);
                ConfigureLifetime(registered, registration.Lifetime);
            }
        }

        /// <summary>
        /// Registers factory registrations.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="registrations">The registrations to register with the container.</param>
        protected override void RegisterFactoryRegistrations(TinyIoCContainer container, IEnumerable<FactoryRegistration> registrations)
        {
            foreach (var registration in registrations)
            {
                FactoryRegistration scopedRegistration = registration;
                var registered = container.Register(registration.RegistrationType, (c, np) =>
                {
                    var context = new FactoryRegistrationContext(c.Resolve);
                    return scopedRegistration.Factory(context);
                });
                ConfigureLifetime(registered, registration.Lifetime);
            }
        }

        private static void ConfigureLifetime(TinyIoCContainer.RegisterOptions options, Lifetime lifetime)
        {
            switch (lifetime)
            {
                case Lifetime.Singleton:
                    options.AsSingleton();
                    break;
                case Lifetime.InstancePerDependency:
                    options.AsMultiInstance();
                    break;
            }
        }
    }
}
