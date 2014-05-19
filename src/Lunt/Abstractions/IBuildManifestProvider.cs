using Lunt.IO;

namespace Lunt
{
    /// <summary>
    /// Service that provides help working with manifest files.
    /// </summary>
    public interface IBuildManifestProvider
    {
        /// <summary>
        /// Loads a build manifest.
        /// </summary>
        /// <param name="path">The manifest file path.</param>
        /// <returns>The loaded manifest; or <c>null</c> if not found.</returns>
        BuildManifest LoadManifest(FilePath path);

        /// <summary>
        /// Saves a build manifest.
        /// </summary>
        /// <param name="path">The manifest file path.</param>
        /// <param name="manifest">The manifest to save.</param>
        void SaveManifest(FilePath path, BuildManifest manifest);
    }
}