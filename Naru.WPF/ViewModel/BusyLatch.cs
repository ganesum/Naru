using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using Naru.Core;

namespace Naru.WPF.ViewModel
{
    public class BusyLatch : IObservable<bool>, IDisposable
    {
        private readonly object _lock = new object();
        private readonly Subject<bool> _isActiveSubject = new Subject<bool>();

        private int _index;

        public IObservable<bool> IsActive
        {
            get { return _isActiveSubject.DistinctUntilChanged().AsObservable(); }
        }

        public IDisposable Subscribe(IObserver<bool> observer)
        {
            Increment();

            return new AnonymousDisposable(() => Decrement());
        }

        private void Increment()
        {
            lock (_lock)
            {
                _index++;

                if (_index > 0)
                {
                    _isActiveSubject.OnNext(true);
                }
            }
        }

        private void Decrement()
        {
            lock (_lock)
            {
                _index--;

                if (_index == 0)
                {
                    _isActiveSubject.OnNext(false);
                }
            }
        }

        public void Dispose()
        {
            lock (_lock)
            {
                _isActiveSubject.OnNext(false);
                _isActiveSubject.OnCompleted();

                _isActiveSubject.Dispose();
            }
        }
    }
}