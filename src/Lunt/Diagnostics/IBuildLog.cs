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
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        void Write(Verbosity verbosity, LogLevel level, string format, params object[] args);
    }
}