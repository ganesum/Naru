using System;
using System.Threading.Tasks;

using Naru.WPF.TPL;

namespace Naru.WPF.MVVM
{
    public interface IDispatcherService
    {
        void ExecuteSyncOnUI(Action action);

        Task<Unit> ExecuteAsyncOnUI(Action action);
    }
}