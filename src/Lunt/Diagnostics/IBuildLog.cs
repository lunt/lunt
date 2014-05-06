namespace Lunt.Diagnostics
{
    /// <summary>
    /// The build log.
    /// </summary>
    public interface IBuildLog
    {
        /// <summary>
        /// Writes a message to the build log using the specified verbosity and log level.
        /// </summary>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="level">The level.</param>
        /// <param name="message">The message.</param>
        void Write(Verbosity verbosity, LogLevel level, string message);
    }
}