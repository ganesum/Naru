using Common.Logging;

namespace Naru.WPF.MVVM
{
    public abstract class Service : IService
    {
        protected readonly ILog Log;

        protected Service(ILog log)
        {
            Log = log;
        }

        public virtual void Dispose()
        { }
    }
}