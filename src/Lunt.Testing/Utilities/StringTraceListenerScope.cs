using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lunt.Testing
{
    public class StringTraceListenerScope : IDisposable
    {
        private bool _disposed;
        private readonly StringTraceListener _listener;

        public List<string> Messages
        {
            get { return _listener.Messages; }
        }

        public StringTraceListenerScope()
        {
            _listener = new StringTraceListener();
            Trace.Listeners.Add(_listener);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                Trace.Listeners.Remove(_listener);
            }
            GC.SuppressFinalize(this);
        }
    }
}
