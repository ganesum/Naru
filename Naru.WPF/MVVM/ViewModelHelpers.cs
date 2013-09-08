using System;

using Microsoft.Practices.Prism.Events;

using Naru.WPF.MVVM.ToolBar;

namespace Naru.WPF.MVVM
{
    public static class ViewModelHelpers
    {
        public static IDisposable SyncViewModelActivationStates(this ISupportActivationState source, ISupportActivationState viewModel)
        {
            EventHandler<DataEventArgs<bool>> sourceOnActivationStateChanged = (s, e) =>
            {
                if (e.Value)
                    viewModel.Activate();
                else
                    viewModel.DeActivate();
            };
            source.OnActivationStateChanged += sourceOnActivationStateChanged;

            return AnonymousDisposable.Create(() => source.OnActivationStateChanged -= sourceOnActivationStateChanged);
        }

        public static IDisposable SyncViewModelDeActivation(this ISupportActivationState source, ISupportActivationState viewModel)
        {
            EventHandler<DataEventArgs<bool>> sourceOnActivationStateChanged = (s, e) =>
            {
                if (!e.Value)
                    viewModel.DeActivate();
            };
            source.OnActivationStateChanged += sourceOnActivationStateChanged;

            return AnonymousDisposable.Create(() => source.OnActivationStateChanged -= sourceOnActivationStateChanged);
        }

        public static IDisposable SyncToolBarItemWithViewModelActivationState(this ISupportActivationState source, params IToolBarItem[] toolBarItems)
        {
            foreach (var toolBarItem in toolBarItems)
            {
                toolBarItem.IsVisible = source.IsActive;
            }

            EventHandler<DataEventArgs<bool>> sourceOnActivationStateChanged = (s, e) =>
            {
                foreach (var toolBarItem in toolBarItems)
                {
                    toolBarItem.IsVisible = e.Value;
                }
            };
            source.OnActivationStateChanged += sourceOnActivationStateChanged;

            return AnonymousDisposable.Create(() => source.OnActivationStateChanged -= sourceOnActivationStateChanged);
        }
    }
}