using System.Diagnostics;

namespace Lunt.Diagnostics
{
    /// <summary>
    /// Writes build log messages to the trace output.
    /// </summary>
    public class TraceBuildLog : IBuildLog
    {
        /// <summary>
        /// Writes a message to the build log using the specified verbosity and log level.
        /// </summary>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="level">The level.</param>
        /// <param name="message">The message.</param>
        public void Write(Verbosity verbosity, LogLevel level, string message)
        {
            Trace.WriteLine(message);
        }
    }
}
