using System;

using Microsoft.Reactive.Testing;

using Naru.Core;
using Naru.WPF.MVVM;
using Naru.WPF.Tests.Scheduler;
using Naru.WPF.ViewModel;

using NUnit.Framework;

namespace Naru.WPF.Tests.ViewModel
{
    [TestFixture]
    public class ReactiveSingleSelectCollectionTests
    {
        [Test]
        public void when_SelectedItem_is_set_then_the_SelectedItemChanged_pumps_the_value()
        {
            var testSchedulerProvider = new TestDispatcherSchedulerProvider();

            var result = Guid.Empty;

            var reactiveSingleSelectCollection = new ReactiveSingleSelectCollection<Guid>(new BindableCollection<Guid>(testSchedulerProvider),
                                                                                          testSchedulerProvider);
            reactiveSingleSelectCollection.SelectedItemChanged
                                          .Subscribe(x => result = x);

            var selectedItem = Guid.NewGuid();

            reactiveSingleSelectCollection.SelectedItem = selectedItem;

            Assert.That(selectedItem.Equals(result));
        }

        [Test]
        public void when_SelectedItem_is_set_then_the_SelectedItem_INPC_is_fired()
        {
            var testSchedulerProvider = new TestDispatcherSchedulerProvider();

            var reactiveSingleSelectCollection = new ReactiveSingleSelectCollection<object>(new BindableCollection<object>(testSchedulerProvider),
                                                                                            testSchedulerProvider);

            var result = false;

            reactiveSingleSelectCollection.PropertyChanged += (sender, args) =>
                                                              {
                                                                  var propertyName = PropertyExtensions.ExtractPropertyName(() => reactiveSingleSelectCollection.SelectedItem);
                                                                  if (args.PropertyName == propertyName)
                                                                  {
                                                                      result = true;
                                                                  }
                                                              };

            reactiveSingleSelectCollection.SelectedItem = new object();
            ((TestScheduler)testSchedulerProvider.Dispatcher.RX).AdvanceBy(1);

            Assert.That(result, Is.True);
        }
    }
}