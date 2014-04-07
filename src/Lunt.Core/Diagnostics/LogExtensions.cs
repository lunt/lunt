﻿// ﻿
// Copyright (c) 2013 Patrik Svensson
// 
// This file is part of Lunt.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
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
                log.Write(verbosity, LogLevel.Error, string.Format(CultureInfo.InvariantCulture, format, args));
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
                log.Write(verbosity, LogLevel.Warning, string.Format(CultureInfo.InvariantCulture, format, args));
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
                log.Write(verbosity, LogLevel.Information, string.Format(CultureInfo.InvariantCulture, format, args));
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
                log.Write(verbosity, LogLevel.Verbose, string.Format(CultureInfo.InvariantCulture, format, args));
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
                log.Write(verbosity, LogLevel.Debug, string.Format(CultureInfo.InvariantCulture, format, args));
            }
        }
    }
}