using System.Threading.Tasks;

using Naru.TPL;

namespace Naru.WPF.ViewModel
{
    public interface ISupportBusy
    {
        bool IsActive { get; }

        string Message { get; }

        void Active(string message);

        void InActive();

        Task<Unit> ActiveAsync(string message);

        Task<Unit> InActiveAsync();
    }
}