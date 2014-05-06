using System.Collections.Generic;
using System.Diagnostics;

namespace Lunt.Testing
{
    public class StringTraceListener : TraceListener
    {
        private readonly List<string> _messages;

        public List<string> Messages
        {
            get { return _messages; }
        }

        public StringTraceListener()
        {
            _messages = new List<string>();
        }

        public override void Write(string message)
        {
            _messages.Add(message);
        }

        public override void WriteLine(string message)
        {
            _messages.Add(message);
        }
    }
}
