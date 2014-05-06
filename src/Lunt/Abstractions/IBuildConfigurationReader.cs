using Lunt.IO;

namespace Lunt
{
    /// <summary>
    /// Provides a mechanism for reading build configurations.
    /// </summary>
    public interface IBuildConfigurationReader
    {
        /// <summary>
        /// Reads a build configuration from an XML file in the file system.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        BuildConfiguration Read(FilePath path);
    }
}