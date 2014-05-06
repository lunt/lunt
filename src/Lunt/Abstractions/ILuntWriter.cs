using System;
using Lunt.IO;

namespace Lunt
{
    /// <summary>
    /// Provides methods and properties for writing a specific managed type.
    /// </summary>
    public interface ILuntWriter : IPipelineComponent
    {
        /// <summary>
        /// Gets the type handled by the writer.
        /// </summary>
        /// <returns></returns>
        Type GetTargetType();

        /// <summary>
        /// Writes an object.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="target">The target.</param>
        /// <param name="value">The value to be written.</param>
        void Write(LuntContext context, IFile target, object value);
    }
}