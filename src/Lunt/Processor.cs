using System;

namespace Lunt
{
    /// <summary>
    /// Provides a base class to use when developing custom processor components.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Processor<T> : Processor<T, T>
    {
    }

    /// <summary>
    /// Provides a base class to use when developing custom processor components.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    public abstract class Processor<TSource, TTarget> : IProcessor
    {
        /// <summary>
        /// Processes the specified input data and returns the result.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="source">Existing content object being processed.</param>
        /// <returns>An object representing the processed input.</returns>
        protected abstract TTarget Process(Context context, TSource source);

        /// <summary>
        /// Gets the object type expected as the input parameter for processing.
        /// </summary>
        /// <returns>
        /// The source type.
        /// </returns>
        public Type GetSourceType()
        {
            return typeof (TSource);
        }

        /// <summary>
        /// Gets the object type expected as a result of processing.
        /// </summary>
        /// <returns>
        /// The target type
        /// </returns>
        public Type GetTargetType()
        {
            return typeof (TTarget);
        }

        object IProcessor.Process(Context context, object source)
        {
            return Process(context, (TSource) source);
        }
    }
}