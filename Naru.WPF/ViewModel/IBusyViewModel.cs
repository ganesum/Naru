using System;
using System.Threading.Tasks;

namespace Naru.WPF.ViewModel
{
    public interface IBusyViewModel : IDisposable
    {
        IObservable<bool> IsActiveChanged { get; }

        bool IsActive { get; }

        string Message { get; }

        BusyLatch BusyLatch { get; }

        void Active(string message);

        void InActive();

        Task ActiveAsync(string message);

        Task InActiveAsync();
    }
}