using System;

namespace Lunt
{
    /// <summary>
    /// A console output writer that writes to the standard output stream.
    /// </summary>
    public sealed class ConsoleWriter : IConsoleWriter
    {
        /// <summary>
        /// Writes the text representation of the specified array of objects, 
        /// followed by the current line terminator to the standard output stream.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        public void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }

        /// <summary>
        /// Sets the forground console color.
        /// </summary>
        /// <param name="color">The foreground color to set.</param>
        public void SetForeground(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }

        /// <summary>
        /// Sets the foreground console color to it's default.
        /// </summary>
        public void ResetColors()
        {
            Console.ResetColor();
        }
    }
}