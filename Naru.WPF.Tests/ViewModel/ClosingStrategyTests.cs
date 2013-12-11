using System;

using Common.Logging.Simple;

using Naru.WPF.ViewModel;

using NUnit.Framework;

namespace Naru.WPF.Tests.ViewModel
{
    [TestFixture]
    public class ClosingStrategyTests
    {
        [Test]
        public void when_Close_is_called_then_Closing_is_called_then_Closed()
        {
            var closingStrategy = new ClosingStrategy(new NoOpLogger());

            var closingCalled = false;
            var closedCalled = false;

            closingStrategy.Closing
                           .Subscribe(_ =>
                                      {
                                          Assert.That(closingCalled, Is.False);
                                          Assert.That(closedCalled, Is.False);

                                          closingCalled = true;
                                      });

            closingStrategy.Closing
                           .Subscribe(_ =>
                           {
                               Assert.That(closingCalled, Is.True);
                               Assert.That(closedCalled, Is.False);

                               closedCalled = true;
                           });

            closingStrategy.Close();

            Assert.That(closingCalled, Is.True);
            Assert.That(closedCalled, Is.True);
        }
    }
}