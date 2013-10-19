using Common.Logging;

namespace Naru.WPF.ViewModel
{
    public abstract class ViewModel : NotifyPropertyChanged, IViewModel
    {
        protected readonly ILog Log;

        protected ViewModel(ILog log)
        {
            Log = log;
        }
    }
}