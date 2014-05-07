using Lunt.IO;

namespace Lunt
{
    /// <summary>
    /// Provides methods and properties for importing an asset to a specific managed type.
    /// </summary>
    public interface IImporter : IPipelineComponent
    {
        /// <summary>
        /// Imports an asset from the specified file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="file">The asset file to import.</param>
        /// <returns></returns>
        object Import(Context context, IFile file);
    }
}