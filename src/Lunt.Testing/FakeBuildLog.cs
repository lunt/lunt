using System.Collections.Generic;
using Lunt.Diagnostics;

namespace Lunt.Testing
{
    public class FakeBuildLog : IBuildLog
    {
        private readonly List<string> _messages;

        public List<string> Messages
        {
            get { return _messages; }
        }

        public FakeBuildLog()
        {
            _messages = new List<string>();
        }

        public void Write(Verbosity verbosity, LogLevel level, string message)
        {
            _messages.Add(message);
        }
    }
}