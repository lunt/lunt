using Lunt.Diagnostics;
using Lunt.IO;

namespace Lake
{
    public sealed class LakeOptions
    {
        public FilePath BuildConfiguration { get; set; }

        public DirectoryPath InputDirectory { get; set; }
        public DirectoryPath OutputDirectory { get; set; }
        public DirectoryPath ProbingDirectory { get; set; }

        public Verbosity Verbosity { get; set; }

        public bool ShowHelp { get; set; }
        public bool ShowVersion { get; set; }

        public bool Rebuild { get; set; }

        public LakeOptions()
        {
            Verbosity = Verbosity.Normal;
        }
    }
}