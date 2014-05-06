using Lake.Diagnostics;
using Lunt.Diagnostics;
using System.Diagnostics;

namespace Lunt.Testing
{
    public sealed class TraceBuildLog : IConsoleBuildLog
    {
        public Verbosity Verbosity { get; set; }

        public TraceBuildLog()
        {
            Verbosity = Verbosity.Verbose;
        }

        public void Write(Verbosity verbosity, LogLevel level, string message)
        {
            Trace.WriteLine(message);
        }
    }
}
