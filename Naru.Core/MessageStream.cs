using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reactive.Subjects;

namespace Naru.Core
{
    public interface IMessageStream
    {
        void Push<TMessage>(TMessage message);

        IObservable<TMessage> Of<TMessage>();
    }

    public class MessageStream : IMessageStream
    {
        private readonly ConcurrentDictionary<Type, object> _subjects = new ConcurrentDictionary<Type, object>();

        public void Push<TMessage>(TMessage message)
        {
            Guard.NotNull(() => message, message);

            var messageType = message.GetType();

            var compatible = _subjects.Keys
                                      .Where(subjectEventType => subjectEventType.IsAssignableFrom(messageType))
                                      .Select(subjectEventType => _subjects[subjectEventType]);

            foreach (dynamic subject in compatible)
            {
                subject.OnNext((dynamic)message);
            }
        }

        public IObservable<TMessage> Of<TMessage>()
        {
            return (IObservable<TMessage>)_subjects.GetOrAdd(typeof(TMessage), type => new Subject<TMessage>());
        }
    }
}