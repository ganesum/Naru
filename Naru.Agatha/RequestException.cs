using System;

namespace Naru.Agatha
{
    public sealed class RequestException : Exception
    {
        public RequestException(string message)
            : base(message)
        { }
    }
}