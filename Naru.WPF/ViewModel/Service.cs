using Common.Logging;

namespace Naru.WPF.ViewModel
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