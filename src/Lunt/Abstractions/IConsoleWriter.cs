using System;

namespace Lunt
{
    /// <summary>
    /// Represents a console output stream.
    /// </summary>
    public interface IConsoleWriter
    {
        /// <summary>
        /// Writes the text representation of the specified array of objects, 
        /// followed by the current line terminator.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        void WriteLine(string format, params object[] args);

        /// <summary>
        /// Sets the forground console color.
        /// </summary>
        /// <param name="color">The foreground color to set.</param>
        void SetForeground(ConsoleColor color);

        /// <summary>
        /// Sets the foreground console color to it's default.
        /// </summary>
        void ResetColors();
    }
}