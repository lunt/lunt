using System;
using System.Collections.Generic;
using Lunt.Diagnostics.Formatting;

namespace Lunt.Diagnostics
{
    /// <summary>
    /// A build log that write colorful messages to a <see cref="IConsoleWriter"/>.
    /// </summary>
    public sealed class ColoredConsoleBuildLog : IBuildLog
    {
        private readonly IConsoleWriter _console;
        private readonly object _lock;
        private readonly IDictionary<LogLevel, ColoredConsolePalette> _palettes;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColoredConsoleBuildLog"/> class.
        /// </summary>
        public ColoredConsoleBuildLog(IConsoleWriter console, bool grayscale = false)
        {
            _console = console;
            _lock = new object();
            _palettes = GetPalette(grayscale);
        }

        /// <summary>
        /// Writes a message to the build log using the specified verbosity and log level.
        /// </summary>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="level">The level.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains one or more objects to format.</param>
        public void Write(Verbosity verbosity, LogLevel level, string format, params object[] args)
        {
            lock (_lock)
            {
                try
                {
                    var palette = _palettes[level];
                    var tokens = FormatParser.Parse(format);
                    foreach (var token in tokens)
                    {
                        SetPalette(token, palette);
                        _console.Write(token.Render(args));
                    }
                    _console.Write(Environment.NewLine);
                }
                finally
                {
                    _console.ResetColors();
                }
            }
        }

        private void SetPalette(FormatToken token, ColoredConsolePalette palette)
        {
            var property = token as PropertyToken;
            if (property != null)
            {
                _console.SetBackground(palette.ArgumentBackground);
                _console.SetForeground(palette.ArgumentForeground);
            }
            else
            {
                _console.SetBackground(palette.Background);
                _console.SetForeground(palette.Foreground);
            }
        }

        private static IDictionary<LogLevel, ColoredConsolePalette> GetPalette(bool grayscale)
        {
            if (grayscale)
            {
                return ColoredConsolePalette.GetGreyscalePalette();
            }
            return ColoredConsolePalette.GetColorfulPalette();
        }
    }
}
