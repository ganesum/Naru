using System;

using NUnit.Framework;

namespace Naru.Core.Tests
{
    [TestFixture]
    public class MessageStreamTests
    {
        [Test]
        public void when_a_subscription_of_T1_is_made_and_Push_of_T2_is_called_then_the_subscription_is_not_called()
        {
            var messageStream = new MessageStream();

            var result = false;

            messageStream.Of<int>()
                         .Subscribe(x => result = true);

            messageStream.Push(true);

            Assert.That(result, Is.False);
        }

        [Test]
        public void when_a_subscription_of_T_is_made_and_Push_of_T_is_called_then_the_subscription_is_called()
        {
            var messageStream = new MessageStream();

            var result = false;

            messageStream.Of<bool>()
                         .Subscribe(x => result = true);

            messageStream.Push(true);

            Assert.That(result, Is.True);
        }

        [Test]
        public void when_a_subscription_of_T_is_made_and_Push_of_T_is_called_twice_then_the_subscription_is_called_twice()
        {
            var messageStream = new MessageStream();

            var result = 1;

            messageStream.Of<bool>()
                         .Subscribe(x => result++);

            messageStream.Push(true);

            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void when_two_subscriptions_of_T_are_made_and_Push_of_T_is_called_then_each_subscription_is_called()
        {
            var messageStream = new MessageStream();

            var result1 = false;

            messageStream.Of<bool>()
                         .Subscribe(x => result1 = true);

            var result2 = false;

            messageStream.Of<bool>()
                         .Subscribe(x => result2 = true);

            messageStream.Push(true);

            Assert.That(result1, Is.True);
            Assert.That(result2, Is.True);
        }
    }
}