using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reactive.Linq;

using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;
using Naru.WPF.ToolBar;

namespace Naru.WPF.ViewModel
{
    public static class ViewModelHelpers
    {
        public static void SetupHeader(this ISupportHeader viewModel, ISchedulerProvider scheduler, string displayName = null, string uri = null)
        {
            var headerViewModel = new HeaderViewModel(scheduler)
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

        public static object GetViewAndBind<TViewModel>(this TViewModel viewModel)
            where TViewModel : IViewModel
        {
            var view = ViewServiceHelper.CreateView(viewModel.GetType());

            ViewServiceHelper.BindViewModel(view, viewModel);

            return view;
        }

        public static IDisposable ConnectINPCProperty<T>(this ObservableProperty<T> observableProperty,
                                                         INotifyPropertyChangedEx viewModel,
                                                         Expression<Func<T>> propertyExpression,
                                                         ISchedulerProvider scheduler)
        {
            var propertyName = PropertyExtensions.ExtractPropertyName(propertyExpression);
            return observableProperty.ValueChanged
                                     .ObserveOn(scheduler.Dispatcher.RX)
                                     .Subscribe(x => viewModel.ConnectINPC(propertyName));
        }

        public static TValue RaiseAndSetIfChanged<TViewModel, TValue>(this TViewModel viewModel,
                                                                      ObservableProperty<TValue> observableProperty,
                                                                      TValue newValue)
            where TViewModel : IViewModel
        {
            // Inspired by ReactiveUI

            if (EqualityComparer<TValue>.Default.Equals(observableProperty.Value, newValue))
            {
                return newValue;
            }

            observableProperty.Value = newValue;

            return newValue;
        }
    }
}