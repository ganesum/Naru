using System;
using System.Reactive.Disposables;

namespace Naru.WPF.ContextMenu
{
    public static class DisposableEx
    {
        public static void AddDisposable(this IDisposable disposable, CompositeDisposable compositeDisposable)
        {
            compositeDisposable.Add(disposable);
        }
    }
}