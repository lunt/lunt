using Lunt;
using Lunt.Diagnostics;

namespace Lake.Diagnostics
{
    /// <summary>
    /// The Lake build log that writes messages to a <see cref="IConsoleWriter"/>.
    /// </summary>
    internal sealed class LakeBuildLog : ILakeBuildLog
    {
        private readonly ConsoleBuildLog _log;

        /// <summary>
        /// Gets or sets the build log verbosity.
        /// </summary>
        public Verbosity Verbosity { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LakeBuildLog" /> class.
        /// </summary>
        /// <param name="console">The console output writer.</param>
        public LakeBuildLog(IConsoleWriter console)
        {
            _log = new ConsoleBuildLog(console);
            Verbosity = Verbosity.Diagnostic;
        }

        /// <summary>
        /// Writes a message to the build log using the specified verbosity and log level.
        /// </summary>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="level">The level.</param>
        /// <param name="message">The message.</param>
        public void Write(Verbosity verbosity, LogLevel level, string message)
        {
            if (verbosity > Verbosity)
            {
                return;
            }            
            _log.Write(verbosity, level, string.Format("[{0}] {1}", level.ToString().Substring(0, 1), message));
        }
    }
}