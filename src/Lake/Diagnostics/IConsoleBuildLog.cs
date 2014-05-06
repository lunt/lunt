using Lunt.Diagnostics;

namespace Lake.Diagnostics
{
    public interface IConsoleBuildLog : IBuildLog
    {
        Verbosity Verbosity { get; set; }
    }
}
