using System;
using System.Windows;

using Naru.WPF.MVVM;
using Naru.WPF.ToolBar;

namespace Naru.WPF.ViewModel
{
    public static class ViewModelHelpers
    {
        public static void SetupHeader(this ISupportHeader viewModel, string displayName = null, string uri = null)
        {
            var headerViewModel = new HeaderViewModel
            {
                DisplayName = displayName,
                ImageName = uri
            };

            viewModel.SetupHeader(headerViewModel);
        }

        public static IDisposable SyncViewModelActivationStates(this ISupportActivationState source, ISupportActivationState viewModel)
        {
            return source.ActivationStateChanged
                         .Subscribe(x =>
                         {
                             if (x)
                                 viewModel.Activate();
                             else
                                 viewModel.DeActivate();
                         });
        }

        public static IDisposable SyncViewModelDeActivation(this ISupportActivationState source, ISupportActivationState viewModel)
        {
            return source.ActivationStateChanged
                         .Subscribe(x =>
                         {
                             if (!x)
                                 viewModel.DeActivate();
                         });
        }

        public static IDisposable SyncToolBarItemWithViewModelActivationState(this ISupportActivationState source, params IToolBarItem[] toolBarItems)
        {
            foreach (var toolBarItem in toolBarItems)
            {
                toolBarItem.IsVisible = source.IsActive;
            }

            return source.ActivationStateChanged
                         .Subscribe(x =>
                         {
                             foreach (var toolBarItem in toolBarItems)
                             {
                                 toolBarItem.IsVisible = x;
                             }
                         });
        }

        public static IDisposable SyncViewModelBusy(this ISupportBusy destination, ISupportBusy source)
        {
            return source.IsActiveChanged
                         .Subscribe(x =>
                         {
                             if (x)
                             {
                                 destination.Active(source.Message);
                             }
                             else
                             {
                                 destination.InActive();
                             }
                         });
        }

        public static void ExecuteOnClosed(this ISupportClosing source, Action action)
        {
            IDisposable supportClosingClosed = null;
            supportClosingClosed = source.Closed
                                         .Subscribe(x =>
                                         {
                                             action();

                                             if (supportClosingClosed != null)
                                             {
                                                 supportClosingClosed.Dispose();
                                             }
                                         });
        }

        public static FrameworkElement GetViewAndBind<TViewModel>(this TViewModel viewModel)
            where TViewModel : IViewModel
        {
            var view = ViewService.CreateView(typeof (TViewModel));

            ViewService.BindViewModel(view, viewModel);

            return view;
        }
    }
}