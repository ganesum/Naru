using Naru.WPF.Command;
using Naru.WPF.MVVM;
using Naru.WPF.ViewModel;

namespace Naru.WPF.Prism.TabControl
{
    public class TabControlHeaderViewModel : IViewModel
    {
        public IViewModel Header { get; private set; }

        public bool CanClose { get; private set; }

        public DelegateCommand CloseCommand { get; private set; }

        public TabControlHeaderViewModel(ISupportHeader viewModel)
        {
            Header = viewModel.Header;

            var supportClose = viewModel as ISupportClosing;
            if (supportClose == null) return;

            CloseCommand = new DelegateCommand(() => supportClose.Close());
            CanClose = supportClose.CanClose();
        }
    }
}