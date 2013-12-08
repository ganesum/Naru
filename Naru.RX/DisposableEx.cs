using System;
using System.Reactive.Disposables;

namespace Naru.RX
{
    public static class DisposableEx
    {
        public static void AddDisposable(this IDisposable disposable, CompositeDisposable compositeDisposable)
        {
            compositeDisposable.Add(disposable);
        }
    }
}