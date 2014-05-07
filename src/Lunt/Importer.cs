using Lunt.IO;

namespace Lunt
{
    /// <summary>
    /// Provides a base class to use when developing custom importer components.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Importer<T> : IImporter
    {
        /// <summary>
        /// Imports an asset from the specified file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="file">The asset file to import.</param>
        /// <returns></returns>
        public abstract T Import(Context context, IFile file);

        object IImporter.Import(Context context, IFile file)
        {
            return Import(context, file);
        }
    }
}