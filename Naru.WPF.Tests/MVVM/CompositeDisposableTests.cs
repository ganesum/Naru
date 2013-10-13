﻿using Naru.Core;
using Naru.WPF.MVVM;

using NUnit.Framework;

namespace Naru.WPF.Tests.MVVM
{
    [TestFixture]
    public class CompositeDisposableTests
    {
        [Test]
        public void when_Dispose_is_called_then_added_IDisposable_is_disposed()
        {
            var disposables = new CompositeDisposable();

            var eventWasFired = false;

            var disposable = AnonymousDisposable.Create(() => eventWasFired = true);
            disposables.Add(disposable);

            Assert.That(eventWasFired, Is.False);

            disposables.Dispose();

            Assert.That(eventWasFired, Is.True);
        }
    }
}