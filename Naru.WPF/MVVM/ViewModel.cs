using Common.Logging;

using Microsoft.Practices.Prism.ViewModel;

namespace Naru.WPF.MVVM
{
    public abstract class ViewModel : NotificationObject, IViewModel
    {
        protected readonly ILog Log;

        protected ViewModel(ILog log)
        {
            Log = log;
        }
    }
}