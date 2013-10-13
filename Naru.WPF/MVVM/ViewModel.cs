using Common.Logging;

namespace Naru.WPF.MVVM
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