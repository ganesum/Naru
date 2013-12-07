using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Naru.WPF.ViewModel
{
    public class ObservableProperty<T>
    {
        private readonly Subject<T> _subject = new Subject<T>();

        private T _latestVaue;

        public T Value
        {
            get { return _latestVaue; }
            set
            {
                _latestVaue = value;
                _subject.OnNext(_latestVaue);
            }
        }

        public IObservable<T> ValueChanged
        {
            get { return _subject.AsObservable(); }
        }

        public ObservableProperty()
        { }

        public ObservableProperty(T initialValue)
        {
            Value = initialValue;
        }
    }
}