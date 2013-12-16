using System;

using Naru.Core;

using NUnit.Framework;

namespace Naru.TPL.Tests
{
    [TestFixture]
    public class EventAsyncTests
    {
        public class StubClassWithEvent
        {
            public event EventHandler NoPayload;

            public void FireNoPayloadEvent()
            {
                NoPayload.SafeInvoke(this);
            }

            public event EventHandler<DataEventArgs<Guid>> Payload;

            public void FirePayloadEvent(Guid payload)
            {
                Payload.SafeInvoke(this, new DataEventArgs<Guid>(payload));
            }
        }

        [Test]
        public void when_event_fires_then_the_action_is_executed()
        {
            var testScheduler = new CurrentThreadTaskScheduler();

            var stub = new StubClassWithEvent();

            var result = false;

            EventAsync.FromEvent(eh => stub.NoPayload += eh, eh => stub.NoPayload -= eh)
                      .Do(() => result = true, testScheduler);

            stub.FireNoPayloadEvent();

            Assert.That(result, Is.True);
        }

        [Test]
        public void when_event_is_fired_twice_then_the_second_time_the_event_is_not_recieved()
        {
            var testScheduler = new CurrentThreadTaskScheduler();

            var stub = new StubClassWithEvent();

            var count = 0;

            EventAsync.FromEvent(eh => stub.NoPayload += eh, eh => stub.NoPayload -= eh)
                      .Do(() => count++, testScheduler);

            stub.FireNoPayloadEvent();
            stub.FireNoPayloadEvent();

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void when_payload_event_fires_then_the_action_is_executed_and_the_payload_is_passed_through()
        {
            var testScheduler = new CurrentThreadTaskScheduler();

            var stub = new StubClassWithEvent();

            var result = Guid.Empty;

            EventAsync.FromEvent<DataEventArgs<Guid>>(eh => stub.Payload += eh, eh => stub.Payload -= eh)
                      .Do(payload => result = payload.Value, testScheduler);

            var expected = Guid.NewGuid();

            stub.FirePayloadEvent(expected);

            Assert.That(result.Equals(expected), Is.True);
        }

        [Test]
        public void when_payload_event_is_fired_twice_then_the_second_time_the_event_is_not_recieved()
        {
            var testScheduler = new CurrentThreadTaskScheduler();

            var stub = new StubClassWithEvent();

            var count = 0;

            EventAsync.FromEvent<DataEventArgs<Guid>>(eh => stub.Payload += eh, eh => stub.Payload -= eh)
                      .Do(_ => count++, testScheduler);

            stub.FirePayloadEvent(Guid.NewGuid());
            stub.FirePayloadEvent(Guid.NewGuid());

            Assert.That(count, Is.EqualTo(1));
        }
    }
}