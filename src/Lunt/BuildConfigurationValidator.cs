using System.Globalization;
using Lunt.IO;

namespace Lunt
{
    internal static class BuildConfigurationValidator
    {
        public static void Validate(IFileSystem fileSystem, BuildConfiguration configuration)
        {
            if (configuration.InputDirectory == null)
            {
                throw new LuntException("Input directory has not been set.");
            }
            if (configuration.InputDirectory.IsRelative)
            {
                throw new LuntException("Input directory cannot be relative.");
            }
            if (configuration.OutputDirectory == null)
            {
                throw new LuntException("Output directory has not been set.");
            }
            if (configuration.OutputDirectory.IsRelative)
            {
                throw new LuntException("Output directory cannot be relative.");
            }

            var inputDirectory = fileSystem.GetDirectory(configuration.InputDirectory);
            if (inputDirectory == null || !inputDirectory.Exists)
            {
                const string format = "Input directory '{0}' does not exist.";
                string message = string.Format(CultureInfo.InvariantCulture, format, configuration.InputDirectory);
                throw new LuntException(message);
            }
        }
    }
}