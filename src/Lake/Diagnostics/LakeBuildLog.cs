using Lunt;
using Lunt.Diagnostics;

namespace Lake.Diagnostics
{
    internal sealed class LakeBuildLog : ILakeBuildLog
    {
        private readonly ColoredConsoleBuildLog _grayscaleLog;
        private readonly ColoredConsoleBuildLog _colorLog;

        public bool Colors { get; set; }
        public Verbosity Verbosity { get; set; }

        public LakeBuildLog(IConsoleWriter console)
        {
            _grayscaleLog = new ColoredConsoleBuildLog(console, grayscale: true);
            _colorLog = new ColoredConsoleBuildLog(console);

            Verbosity = Verbosity.Diagnostic;
        }

        public void Write(Verbosity verbosity, LogLevel level, string format, params object[] args)
        {
            if (verbosity > Verbosity)
            {
                return;
            }

            // Prefix the format with the log level.
            format = string.Concat("[", level.ToString().Substring(0, 1), "] ", format);

            if (Colors)
            {
                _colorLog.Write(verbosity, level, format, args);
            }
            else
            {
                _grayscaleLog.Write(verbosity, level, format, args);
            }
        }
    }
}