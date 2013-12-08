using System;
using System.Reactive.Linq;

namespace Naru.RX
{
    public static class ObservableEx
    {
        public static IObservable<Tuple<T1, T2>> WhenAny<T1, T2>(IObservable<T1> property1,
                                                                 IObservable<T2> property2)
        {
            return property1
                .CombineLatest(property2, (t1, t2) => Tuple.Create(t1, t2));
        }

        public static IObservable<Tuple<T1, T2, T3>> WhenAny<T1, T2, T3>(IObservable<T1> property1,
                                                                 IObservable<T2> property2,
                                                                 IObservable<T3> property3)
        {
            return property1
                .CombineLatest(property2, (t1, t2) => Tuple.Create(t1, t2))
                .CombineLatest(property3, (t, t3) => Tuple.Create(t.Item1, t.Item2, t3));
        }
    }
}