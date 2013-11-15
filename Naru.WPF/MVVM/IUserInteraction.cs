using System;
using System.Threading.Tasks;

namespace Naru.WPF.MVVM
{
    public interface IUserInteraction
    {
        void RegisterHandler(Func<IUserInteractionViewModel, Task> modalHandler);

        Task ShowModalAsync(IUserInteractionViewModel viewModelBuilder);
    }
}