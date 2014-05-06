using System;
using Lunt.IO;

namespace Lunt
{
    /// <summary>
    /// Provides a base class to use when developing custom writer components.
    /// </summary>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    public abstract class LuntWriter<TTarget> : ILuntWriter
    {
        /// <summary>
        /// Compiles an object.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="target">The target.</param>
        /// <param name="value">The value.</param>
        public abstract void Write(LuntContext context, IFile target, TTarget value);

        /// <summary>
        /// Gets the type handled by the writer.
        /// </summary>
        /// <returns></returns>
        public Type GetTargetType()
        {
            return typeof (TTarget);
        }

        void ILuntWriter.Write(LuntContext context, IFile target, object value)
        {
            Write(context, target, (TTarget) value);
        }
    }
}