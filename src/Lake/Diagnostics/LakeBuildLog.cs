using Lunt;
using Lunt.Diagnostics;

namespace Lake.Diagnostics
{
    /// <summary>
    /// The Lake build log that writes messages to a <see cref="IConsoleWriter"/>.
    /// </summary>
    internal sealed class LakeBuildLog : ILakeBuildLog
    {
        private readonly ColoredConsoleBuildLog _grayscaleLog;
        private readonly ColoredConsoleBuildLog _colorLog;

        /// <summary>
        /// Gets or sets a value indicating whether colors should be used.
        /// </summary>
        /// <value>
        ///   <c>true</c> if colors should be used; otherwise, <c>false</c>.
        /// </value>
        public bool Colors { get; set; }

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
            _grayscaleLog = new ColoredConsoleBuildLog(console, grayscale: true);
            _colorLog = new ColoredConsoleBuildLog(console);

            Verbosity = Verbosity.Diagnostic;
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
            if (verbosity > Verbosity)
            {
                return;
            }

            // Prefix the format with the log level.
            format = string.Concat("[", level.ToString().Substring(0, 1), "] ", format);

            if (Colors)
            {
                _colorLog.Write(verbosity, level, format, args);
            }
            else
            {
                _grayscaleLog.Write(verbosity, level, format, args);
            }
        }
    }
}