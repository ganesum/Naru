﻿using NUnit.Framework;

namespace Naru.Core.Tests
{
    [TestFixture]
    public class AnonymousDisposableTests
    {
        [Test]
        public void when_Dispose_is_called_Action_is_called()
        {
            var actionWasCalled = false;

            var anonymousDisposable = new AnonymousDisposable(() => actionWasCalled = true);

            Assert.That(actionWasCalled, Is.False);

            anonymousDisposable.Dispose();

            Assert.That(actionWasCalled, Is.True);
        }
    }
}