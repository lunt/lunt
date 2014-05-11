using System;

namespace Lunt.Diagnostics
{
    /// <summary>
    /// A build log that write messages to a <see cref="IConsoleWriter"/>.
    /// </summary>
    public sealed class ConsoleBuildLog : IBuildLog
    {
        private readonly IConsoleWriter _writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleBuildLog"/> class.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public ConsoleBuildLog(IConsoleWriter writer)
        {
            _writer = writer;
        }

        /// <summary>
        /// Writes a message to the build log using the specified verbosity and log level.
        /// </summary>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="level">The level.</param>
        /// <param name="message">The message.</param>
        public void Write(Verbosity verbosity, LogLevel level, string message)
        {
            try
            {
                _writer.SetForeground(GetColor(level));
                _writer.WriteLine(message);
            }
            finally
            {
                _writer.ResetColors();
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
