using System;
using Lunt;
using Lunt.Diagnostics;

namespace Lake.Diagnostics
{
    /// <summary>
    /// A build log that write messages to a <see cref="IConsoleWriter"/>.
    /// </summary>
    internal sealed class ConsoleBuildLog : IConsoleBuildLog
    {
        private readonly IConsoleWriter _console;

        /// <summary>
        /// Gets or sets the build log verbosity.
        /// </summary>
        public Verbosity Verbosity { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleBuildLog" /> class.
        /// </summary>
        /// <param name="console">The console output writer.</param>
        public ConsoleBuildLog(IConsoleWriter console)
        {
            _console = console;
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

            try
            {
                _console.SetForeground(GetColor(level));
                _console.WriteLine("[{0}] {1}", level.ToString().Substring(0, 1), message);
            }
            finally
            {
                _console.ResetColors();
            }
        }

        private static ConsoleColor GetColor(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Error:
                    return ConsoleColor.Red;
                case LogLevel.Warning:
                    return ConsoleColor.Yellow;
                case LogLevel.Information:
                    return ConsoleColor.White;
                case LogLevel.Verbose:
                    return ConsoleColor.Gray;
                case LogLevel.Debug:
                    return ConsoleColor.DarkGray;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}