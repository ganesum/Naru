using System.Threading.Tasks;

using Naru.WPF.TPL;

namespace Naru.WPF.MVVM
{
    public interface ISupportAsync
    {
        bool IsBusy { get; }
        string BusyMessage { get; }
        void Busy(string message);
        void Idle();
        Task<Unit> BusyAsync(string message);
        Task<Unit> IdleAsync();
    }
}