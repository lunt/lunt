using Lunt.IO;

namespace Lunt
{
    /// <summary>
    /// Provides a base class to use when developing custom importer components.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class LuntImporter<T> : ILuntImporter
    {
        /// <summary>
        /// Imports an asset from the specified file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="file">The asset file to import.</param>
        /// <returns></returns>
        public abstract T Import(LuntContext context, IFile file);

        object ILuntImporter.Import(LuntContext context, IFile file)
        {
            return Import(context, file);
        }
    }
}