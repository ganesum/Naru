using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Reactive.Testing;

using Naru.WPF.MVVM;
using Naru.WPF.Tests.Scheduler;
using Naru.WPF.ViewModel;

using NUnit.Framework;

namespace Naru.WPF.Tests.ViewModel
{
    [TestFixture]
    public class ReactiveMultiSelectCollectionTests
    {
        [Test]
        public void when_SelectedItem_is_set_then_the_SelectedItemChanged_pumps_the_value()
        {
            var testSchedulerProvider = new TestSchedulerProvider();

            var result = new List<int>();

            var reactiveMultiSelectCollection = new ReactiveMultiSelectCollection<int>(new BindableCollection<int>(testSchedulerProvider),
                                                                                       testSchedulerProvider);
            reactiveMultiSelectCollection.SelectedItemsChanged
                                          .Subscribe(x => result = x.ToList());

            var selectedItem = Enumerable.Range(0,1).ToList();

            reactiveMultiSelectCollection.SelectedItems = selectedItem;

            Assert.That(selectedItem.SequenceEqual(result));
        }

        [Test]
        public void when_SelectedItem_is_set_then_the_SelectedItems_INPC_is_fired()
        {
            var testSchedulerProvider = new TestSchedulerProvider();

            var reactiveMultiSelectCollection = new ReactiveMultiSelectCollection<int>(new BindableCollection<int>(testSchedulerProvider),
                                                                                       testSchedulerProvider);

            var result = false;

            reactiveMultiSelectCollection.PropertyChanged += (sender, args) =>
                                                             {
                                                                 var propertyName = PropertyExtensions.ExtractPropertyName(() => reactiveMultiSelectCollection.SelectedItems);
                                                                 if (args.PropertyName == propertyName)
                                                                 {
                                                                     result = true;
                                                                 }
                                                             };

            reactiveMultiSelectCollection.SelectedItems = Enumerable.Range(0, 1).ToList();
            ((TestScheduler)testSchedulerProvider.Dispatcher.RX).AdvanceBy(1);

            Assert.That(result, Is.True);
        }
    }
}