using System;

namespace Lunt
{
    /// <summary>
    /// Represents a console output stream.
    /// </summary>
    public interface IConsoleWriter
    {
        /// <summary>
        /// Writes the text representation of the specified array of objects.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        void Write(string format, params object[] args);

        /// <summary>
        /// Writes the text representation of the specified array of objects, 
        /// followed by the current line terminator.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        void WriteLine(string format, params object[] args);

        /// <summary>
        /// Sets the background console color.
        /// </summary>
        /// <param name="color">The color.</param>
        void SetBackground(ConsoleColor color);

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