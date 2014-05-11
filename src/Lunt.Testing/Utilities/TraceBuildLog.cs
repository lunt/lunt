using Lake.Diagnostics;
using Lunt.Diagnostics;
using System.Diagnostics;

namespace Lunt.Testing
{
    public sealed class TraceBuildLog : ILakeBuildLog
    {
        public Verbosity Verbosity { get; set; }

        public TraceBuildLog()
        {
            Verbosity = Verbosity.Verbose;
        }

        public void Write(Verbosity verbosity, LogLevel level, string format, params object[] args)
        {
            Trace.WriteLine(string.Format(format, args));
        }
    }
}
