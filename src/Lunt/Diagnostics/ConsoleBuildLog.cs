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
using System;

namespace Lunt.Diagnostics
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
            if (verbosity > this.Verbosity)
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