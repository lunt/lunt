using Lunt.Diagnostics;

namespace Lake.Diagnostics
{
    public interface ILakeBuildLog : IBuildLog
    {
        bool Colors { get; set; }
        Verbosity Verbosity { get; set; }
    }
}
