using System;

namespace Lunt
{
    /// <summary>
    /// Provides methods and properties for processing or converting of a specific managed type.
    /// </summary>
    public interface IProcessor : IPipelineComponent
    {
        /// <summary>
        /// Gets the object type expected as the input parameter for processing.
        /// </summary>
        /// <returns>The source type.</returns>
        Type GetSourceType();

        /// <summary>
        /// Gets the object type expected as a result of processing.
        /// </summary>
        /// <returns>The target type</returns>
        Type GetTargetType();

        /// <summary>
        /// Processes the specified input data and returns the result.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="source">Existing content object being processed.</param>
        /// <returns>An object representing the processed input.</returns>
        object Process(Context context, object source);
    }
}