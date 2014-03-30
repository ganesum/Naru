using System.Threading.Tasks;
using System.Windows;
using Common.Logging;

using Naru.TPL;
using Naru.WPF.Scheduler;
using Naru.WPF.TPL;
using Naru.WPF.ViewModel;
using Naru.WPF.Windows.Controls;

namespace Naru.WPF.MVVM
{
    public class ViewService : IViewService
    {
        private readonly ILog _log;
        private readonly IDispatcherSchedulerProvider _scheduler;

        public ViewService(ILog log, IDispatcherSchedulerProvider scheduler)
        {
            _log = log;
            _scheduler = scheduler;
        }

        public void ShowModal(IViewModel viewModel)
        {
            _scheduler.Dispatcher.ExecuteSync(() => ShowModalInternal(viewModel));
        }

        public Task ShowModalAsync(IViewModel viewModel)
        {
            return Task.Factory.StartNew(() => ShowModalInternal(viewModel), _scheduler.Dispatcher.TPL);
        }

        private void ShowModalInternal(IViewModel viewModel)
        {
            _log.Debug(string.Format("Creating View and Bind for ViewModel - {0}", viewModel.GetType().FullName));

            var view = viewModel.GetViewAndBind();

            var window = view as Window;
            if (window != null)
            {
                ConnectUpActivation(viewModel, window);
                ConnectUpClosing(viewModel, window);

                window.Owner = Application.Current.MainWindow;
                window.ShowDialog();
            }
            else
            {
                window = new ModernWindow
                {
                    Content = view,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    WindowStyle = WindowStyle.ToolWindow,
                    Owner = Application.Current.MainWindow
                };

                var supportHeader = viewModel as ISupportHeader;
                if (supportHeader != null && supportHeader.Header != null)
                {
                    window.Title = supportHeader.Header.ToString();
                }

                ConnectUpActivation(viewModel, window);
                ConnectUpClosing(viewModel, window);

                window.ShowDialog();
            }
        }

        private void ConnectUpActivation(IViewModel viewModel, Window window)
        {
            var supportActivationState = viewModel as ISupportActivationState;
            if (supportActivationState == null) return;

            RoutedEventAsync.FromRoutedEvent(eh => window.Loaded += eh, eh => window.Loaded -= eh)
                            .Do(() => supportActivationState.ActivationStateViewModel.Activate(), _scheduler.Dispatcher.TPL);
        }

        private void ConnectUpClosing(IViewModel viewModel, Window window)
        {
            var supportClosing = viewModel as ISupportClosing;
            if (supportClosing == null) return;

            // ViewModel is closed, so close the Window
            supportClosing.ExecuteOnClosed(() => Task.Factory.StartNew(() => window.Close(), _scheduler.Dispatcher.TPL));

            // Window is closed, so close the ViewModel
            EventAsync.FromEvent(eh => window.Closed += eh, eh => window.Closed -= eh)
                      .Do(() => supportClosing.ClosingStrategy.Close(), _scheduler.Dispatcher.TPL);
        }
    }
}