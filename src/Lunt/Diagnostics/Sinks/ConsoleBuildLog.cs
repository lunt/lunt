namespace Lunt.Diagnostics
{
    /// <summary>
    /// A build log that write messages to a <see cref="IConsoleWriter"/>.
    /// </summary>
    public sealed class ConsoleBuildLog : IBuildLog
    {
        private readonly IConsoleWriter _console;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleBuildLog"/> class.
        /// </summary>
        /// <param name="console">The console writer.</param>
        public ConsoleBuildLog(IConsoleWriter console)
        {
            _console = console;
        }

        /// <summary>
        /// Writes a message to the build log using the specified verbosity and log level.
        /// </summary>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="level">The level.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public void Write(Verbosity verbosity, LogLevel level, string format, params object[] args)
        {
            _console.WriteLine(format, args);
        }
    }
}
