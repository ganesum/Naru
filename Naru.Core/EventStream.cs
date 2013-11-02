using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reactive.Subjects;

namespace Naru.Core
{
    public interface IEventStream
    {
        void Push<TEvent>(TEvent @event);

        IObservable<TEvent> Of<TEvent>();
    }

    public class EventStream : IEventStream
    {
        private readonly ConcurrentDictionary<Type, object> _subjects = new ConcurrentDictionary<Type, object>();

        public void Push<TEvent>(TEvent @event)
        {
            Guard.NotNull(() => @event, @event);

            var eventType = @event.GetType();

            var compatible = _subjects.Keys
                .Where(subjectEventType => subjectEventType.IsAssignableFrom(eventType))
                .Select(subjectEventType => _subjects[subjectEventType]);

            foreach (dynamic subject in compatible)
            {
                subject.OnNext((dynamic)@event);
            }
        }

        public IObservable<TEvent> Of<TEvent>()
        {
            return (IObservable<TEvent>)_subjects.GetOrAdd(typeof(TEvent), type => new Subject<TEvent>());
        }
    }
}