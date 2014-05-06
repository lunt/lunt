using System.IO;
using Lunt.IO;

namespace Lunt
{
    /// <summary>
    /// Provides a mechanism to load and save manifest files.
    /// </summary>
    public sealed class BuildManifestProvider : IBuildManifestProvider
    {
        /// <summary>
        /// Loads a manifest file.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The manifest file path.</param>
        /// <returns>The loaded manifest.</returns>
        public BuildManifest LoadManifest(IFileSystem fileSystem, FilePath path)
        {
            var manifestFile = fileSystem.GetFile(path);
            if (manifestFile.Exists)
            {
                using (var stream = manifestFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    return BuildManifest.Load(stream);
                }
            }
            return null;
        }

        /// <summary>
        /// Saves a manifest to a file.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The file path where the manifest will be saved.</param>
        /// <param name="manifest">The manifest to be saved.</param>
        public void SaveManifest(IFileSystem fileSystem, FilePath path, BuildManifest manifest)
        {
            var file = fileSystem.GetFile(path);
            using (var stream = file.Open(FileMode.Create, FileAccess.Write, FileShare.None))
            {
                manifest.Save(stream);
            }
        }
    }
}