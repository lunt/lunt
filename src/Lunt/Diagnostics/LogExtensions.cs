using System.Globalization;

namespace Lunt.Diagnostics
{
    /// <summary>
    /// Provides extension methods to the <see cref="IBuildLog"/> class.
    /// </summary>
    public static class LogExtensions
    {
        /// <summary>
        /// Logs an error to the build log.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public static void Error(this IBuildLog log, string format, params object[] args)
        {
            Error(log, Verbosity.Quiet, format, args);
        }

        /// <summary>
        /// Logs an error to the build log using the specified verbosity.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public static void Error(this IBuildLog log, Verbosity verbosity, string format, params object[] args)
        {
            if (log != null)
            {
                log.Write(verbosity, LogLevel.Error, format, args);
            }
        }

        /// <summary>
        /// Logs a warning to the build log.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public static void Warning(this IBuildLog log, string format, params object[] args)
        {
            Warning(log, Verbosity.Minimal, format, args);
        }

        /// <summary>
        /// Logs an warning to the build log using the specified verbosity.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public static void Warning(this IBuildLog log, Verbosity verbosity, string format, params object[] args)
        {
            if (log != null)
            {
                log.Write(verbosity, LogLevel.Warning, format, args);
            }
        }

        /// <summary>
        /// Logs information to the build log.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public static void Information(this IBuildLog log, string format, params object[] args)
        {
            Information(log, Verbosity.Normal, format, args);
        }

        /// <summary>
        /// Logs information to the build log using the specified verbosity.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public static void Information(this IBuildLog log, Verbosity verbosity, string format, params object[] args)
        {
            if (log != null)
            {
                log.Write(verbosity, LogLevel.Information, format, args);
            }
        }

        /// <summary>
        /// Logs verbose information to the build log.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public static void Verbose(this IBuildLog log, string format, params object[] args)
        {
            Verbose(log, Verbosity.Verbose, format, args);
        }

        /// <summary>
        /// Logs verbose information to the build log using the specified verbosity.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public static void Verbose(this IBuildLog log, Verbosity verbosity, string format, params object[] args)
        {
            if (log != null)
            {
                log.Write(verbosity, LogLevel.Verbose, format, args);
            }
        }

        /// <summary>
        /// Logs debug information to the build log.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public static void Debug(this IBuildLog log, string format, params object[] args)
        {
            Debug(log, Verbosity.Diagnostic, format, args);
        }

        /// <summary>
        /// Logs debug information to the build log using the specified verbosity.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public static void Debug(this IBuildLog log, Verbosity verbosity, string format, params object[] args)
        {
            if (log != null)
            {
                log.Write(verbosity, LogLevel.Debug, format, args);
            }
        }
    }
}