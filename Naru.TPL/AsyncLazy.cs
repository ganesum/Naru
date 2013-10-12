using System;
using System.Threading.Tasks;

namespace Naru.TPL
{
    public class AsyncLazy<T> : Lazy<Task<T>>
    {
        // http://blogs.msdn.com/b/pfxteam/archive/2011/01/15/10116210.aspx

        public AsyncLazy(Func<T> valueFactory) :
            base(() => Task.Factory.StartNew(valueFactory)) 
        { }

        public AsyncLazy(Func<Task<T>> taskFactory) :
            base(() => Task.Factory.StartNew(() => taskFactory()).Unwrap()) { }
    }
}