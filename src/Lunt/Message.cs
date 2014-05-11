using System;

namespace Lunt
{
    /// <summary>
    /// Represents a message.
    /// </summary>
    public sealed class Message
    {
        private readonly string _format;
        private readonly object[] _arguments;

        /// <summary>
        /// An empty message.
        /// </summary>
        public static readonly Message Empty = new Message(string.Empty);

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public Message(string format, params object[] args)
        {
            if (format == null)
            {
                throw new ArgumentNullException("format");
            }
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            _format = format;
            _arguments = args;
        }

        /// <summary>
        /// Gets the composite format string.
        /// </summary>
        /// <value>The format.</value>
        public string Format
        {
            get { return _format; }
        }

        /// <summary>
        /// Gets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public object[] Arguments
        {
            get { return _arguments; }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return string.Format(_format, _arguments);
        }
    }
}
