using System;
using System.Threading.Tasks;

namespace Naru.WPF.MVVM
{
    public class UserInteraction : IUserInteraction
    {
        private Func<IUserInteractionViewModel, Task> _modalHandler;

        public void RegisterHandler(Func<IUserInteractionViewModel, Task> modalHandler)
        {
            _modalHandler = modalHandler;
        }

        public Task ShowModalAsync(IUserInteractionViewModel viewModel)
        {
            return _modalHandler(viewModel);
        }
    }
}