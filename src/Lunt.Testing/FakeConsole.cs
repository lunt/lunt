using System;
using System.Collections.Generic;
using System.Globalization;

namespace Lunt.Testing
{
    public class FakeConsole : IConsoleWriter
    {
        private readonly List<string> _content;

        public FakeConsole()
        {
            _content = new List<string>();
        }

        public List<string> Content
        {
            get { return _content; }
        }

        public void WriteLine(string format, params object[] args)
        {
            Content.Add(string.Format(CultureInfo.InvariantCulture, format, args));
        }

        public void SetForeground(ConsoleColor color)
        {
        }

        public void ResetColors()
        {
        }
    }
}