using System;
using System.Diagnostics;

namespace Naru.Core.Diagnostics
{
    public class Tracing : IDisposable
    {
        private readonly TraceSource _traceSource;
        private readonly string _name;

        public TraceSource TraceSource
        {
            get { return _traceSource; }
        }

        public Tracing(TraceSource traceSource, string name)
        {
            _traceSource = traceSource;
            _name = name;

            _traceSource.TraceInformation(string.Format("Entering {0}", _name));
        }

        public void Dispose()
        {
            _traceSource.TraceInformation(string.Format("Leaving {0}", _name));
        }
    }
}