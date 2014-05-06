using System;
using System.Runtime.Serialization;

namespace Lunt
{
    /// <summary>
    /// Represents errors that occur during execution.
    /// </summary>
    [Serializable]
    public sealed class LuntException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LuntException" /> class.
        /// </summary>
        public LuntException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LuntException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public LuntException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LuntException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public LuntException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="LuntException" /> class from being created.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        private LuntException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}