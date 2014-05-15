namespace Lunt.Bootstrapping
{
    /// <summary>
    /// Represents a container registration.
    /// </summary>
    public abstract class ContainerRegistration
    {
        private readonly Lifetime _lifetime;

        /// <summary>
        /// Gets the lifetime of the registration.
        /// </summary>
        /// <value>The lifetime.</value>
        public Lifetime Lifetime
        {
            get { return _lifetime; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerRegistration"/> class.
        /// </summary>
        /// <param name="lifetime">The lifetime.</param>
        protected ContainerRegistration(Lifetime lifetime)
        {
            _lifetime = lifetime;
        }
    }
}