using Lunt.Diagnostics;

namespace Lake.Diagnostics
{
    public interface ILakeBuildLog : IBuildLog
    {
        Verbosity Verbosity { get; set; }
    }
}
