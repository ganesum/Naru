using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

using Naru.WPF.MVVM;
using Naru.WPF.Tests.Scheduler;

using NUnit.Framework;

namespace Naru.WPF.Tests.MVVM
{
    [TestFixture]
    public class BindableCollectionTests
    {
        [Test]
        public void when_Add_is_called_then_Add_event_is_fired()
        {
            var testSchedulerProvider = new TestDispatcherSchedulerProvider();

            var result = false;

            var bindableCollection = new BindableCollection<int>(testSchedulerProvider);

            bindableCollection.CollectionChanged += (sender, args) =>
                                                    {
                                                        if (args.Action == NotifyCollectionChangedAction.Add)
                                                        {
                                                            result = true;
                                                        }
                                                    };

            bindableCollection.Add(1);

            Assert.That(result, Is.True);
        }

        [Test]
        public void when_Add_is_called_and_IsNotifying_is_false_then_Add_event_is_not_fired()
        {
            var testSchedulerProvider = new TestDispatcherSchedulerProvider();

            var result = false;

            var bindableCollection = new BindableCollection<int>(testSchedulerProvider);

            bindableCollection.CollectionChanged += (sender, args) =>
                                                    {
                                                        if (args.Action == NotifyCollectionChangedAction.Add)
                                                        {
                                                            result = true;
                                                        }
                                                    };

            bindableCollection.IsNotifying = false;

            bindableCollection.Add(1);

            Assert.That(result, Is.False);
        }

        [Test]
        public void when_Remove_is_called_then_Remove_event_is_fired()
        {
            var testSchedulerProvider = new TestDispatcherSchedulerProvider();

            var result = false;

            var bindableCollection = new BindableCollection<Guid>(testSchedulerProvider);

            var newGuid = Guid.NewGuid();

            bindableCollection.Add(newGuid);

            bindableCollection.CollectionChanged += (sender, args) =>
                                                    {
                                                        if (args.Action == NotifyCollectionChangedAction.Remove)
                                                        {
                                                            result = true;
                                                        }
                                                    };

            bindableCollection.Remove(newGuid);

            Assert.That(result, Is.True);
        }

        [Test]
        public void when_Remove_is_called_and_IsNotifying_is_false_then_Remove_event_is_fired()
        {
            var testSchedulerProvider = new TestDispatcherSchedulerProvider();

            var result = false;

            var bindableCollection = new BindableCollection<Guid>(testSchedulerProvider);

            var newGuid = Guid.NewGuid();

            bindableCollection.Add(newGuid);

            bindableCollection.CollectionChanged += (sender, args) =>
                                                    {
                                                        if (args.Action == NotifyCollectionChangedAction.Remove)
                                                        {
                                                            result = true;
                                                        }
                                                    };

            bindableCollection.IsNotifying = false;

            bindableCollection.Remove(newGuid);

            Assert.That(result, Is.False);
        }

        [Test]
        public void when_Clear_is_called_then_Reset_event_is_fired()
        {
            var testSchedulerProvider = new TestDispatcherSchedulerProvider();

            var result = false;

            var bindableCollection = new BindableCollection<Guid>(testSchedulerProvider);

            var newGuid = Guid.NewGuid();

            bindableCollection.Add(newGuid);

            bindableCollection.CollectionChanged += (sender, args) =>
                                                    {
                                                        if (args.Action == NotifyCollectionChangedAction.Reset)
                                                        {
                                                            result = true;
                                                        }
                                                    };

            bindableCollection.Clear();

            Assert.That(result, Is.True);
        }

        [Test]
        public void when_Clear_is_called_and_IsNotifying_is_false_then_Reset_event_is_fired()
        {
            var testSchedulerProvider = new TestDispatcherSchedulerProvider();

            var result = false;

            var bindableCollection = new BindableCollection<Guid>(testSchedulerProvider);

            var newGuid = Guid.NewGuid();

            bindableCollection.Add(newGuid);

            bindableCollection.CollectionChanged += (sender, args) =>
                                                    {
                                                        if (args.Action == NotifyCollectionChangedAction.Reset)
                                                        {
                                                            result = true;
                                                        }
                                                    };

            bindableCollection.IsNotifying = false;

            bindableCollection.Clear();

            Assert.That(result, Is.False);
        }

        [Test]
        public void when_ClearAsync_is_called_then_Reset_event_is_fired()
        {
            var testSchedulerProvider = new TestDispatcherSchedulerProvider();

            var result = false;

            var bindableCollection = new BindableCollection<Guid>(testSchedulerProvider);

            var newGuid = Guid.NewGuid();

            bindableCollection.Add(newGuid);

            bindableCollection.CollectionChanged += (sender, args) =>
                                                    {
                                                        if (args.Action == NotifyCollectionChangedAction.Reset)
                                                        {
                                                            result = true;
                                                        }
                                                    };

            bindableCollection.ClearAsync();

            Assert.That(result, Is.True);
        }

        [Test]
        public void when_ClearAsync_is_called_and_IsNotifying_is_false_then_Reset_event_is_fired()
        {
            var testSchedulerProvider = new TestDispatcherSchedulerProvider();

            var result = false;

            var bindableCollection = new BindableCollection<Guid>(testSchedulerProvider);

            var newGuid = Guid.NewGuid();

            bindableCollection.Add(newGuid);

            bindableCollection.CollectionChanged += (sender, args) =>
                                                    {
                                                        if (args.Action == NotifyCollectionChangedAction.Reset)
                                                        {
                                                            result = true;
                                                        }
                                                    };

            bindableCollection.IsNotifying = false;

            bindableCollection.ClearAsync();

            Assert.That(result, Is.False);
        }

        [Test]
        public void when_AddRange_is_called_then_Reset_event_fired()
        {
            var testSchedulerProvider = new TestDispatcherSchedulerProvider();

            var result = false;

            var bindableCollection = new BindableCollection<int>(testSchedulerProvider);

            bindableCollection.CollectionChanged += (sender, args) =>
                                                    {
                                                        if (args.Action == NotifyCollectionChangedAction.Reset)
                                                        {
                                                            result = true;
                                                        }
                                                    };

            bindableCollection.AddRange(Enumerable.Range(0,1));

            Assert.That(result, Is.True);
        }

        [Test]
        public void when_AddRange_is_called_and_IsNotifying_is_false_then_Reset_event_fired()
        {
            var testSchedulerProvider = new TestDispatcherSchedulerProvider();

            var result = false;

            var bindableCollection = new BindableCollection<int>(testSchedulerProvider);

            bindableCollection.CollectionChanged += (sender, args) =>
                                                    {
                                                        if (args.Action == NotifyCollectionChangedAction.Reset)
                                                        {
                                                            result = true;
                                                        }
                                                    };

            bindableCollection.IsNotifying = false;

            bindableCollection.AddRange(Enumerable.Range(0, 1));

            Assert.That(result, Is.False);
        }

        [Test]
        public void when_AddRangeAsync_is_called_then_Reset_event_fired()
        {
            var testSchedulerProvider = new TestDispatcherSchedulerProvider();

            var result = false;

            var bindableCollection = new BindableCollection<int>(testSchedulerProvider);

            bindableCollection.CollectionChanged += (sender, args) =>
                                                    {
                                                        if (args.Action == NotifyCollectionChangedAction.Reset)
                                                        {
                                                            result = true;
                                                        }
                                                    };

            bindableCollection.AddRangeAsync(Enumerable.Range(0, 1));

            Assert.That(result, Is.True);
        }

        [Test]
        public void when_AddRangeAsync_is_called_and_IsNotifying_is_false_then_Reset_event_fired()
        {
            var testSchedulerProvider = new TestDispatcherSchedulerProvider();

            var result = false;

            var bindableCollection = new BindableCollection<int>(testSchedulerProvider);

            bindableCollection.CollectionChanged += (sender, args) =>
                                                    {
                                                        if (args.Action == NotifyCollectionChangedAction.Reset)
                                                        {
                                                            result = true;
                                                        }
                                                    };

            bindableCollection.IsNotifying = false;

            bindableCollection.AddRangeAsync(Enumerable.Range(0, 1));

            Assert.That(result, Is.False);
        }

        [Test]
        public void when_RemoveRange_is_called_then_Reset_event_fired()
        {
            var testSchedulerProvider = new TestDispatcherSchedulerProvider();

            var result = false;

            var bindableCollection = new BindableCollection<int>(testSchedulerProvider);

            var items = Enumerable.Range(0, 1).ToList();

            bindableCollection.AddRange(items);

            bindableCollection.CollectionChanged += (sender, args) =>
                                                    {
                                                        if (args.Action == NotifyCollectionChangedAction.Reset)
                                                        {
                                                            result = true;
                                                        }
                                                    };

            bindableCollection.RemoveRange(items);

            Assert.That(result, Is.True);
        }

        [Test]
        public void when_RemoveRange_is_called_and_IsNotifying_is_false_then_Reset_event_fired()
        {
            var testSchedulerProvider = new TestDispatcherSchedulerProvider();

            var result = false;

            var bindableCollection = new BindableCollection<int>(testSchedulerProvider);

            var items = Enumerable.Range(0, 1).ToList();

            bindableCollection.AddRange(items);

            bindableCollection.CollectionChanged += (sender, args) =>
                                                    {
                                                        if (args.Action == NotifyCollectionChangedAction.Reset)
                                                        {
                                                            result = true;
                                                        }
                                                    };

            bindableCollection.IsNotifying = false;

            bindableCollection.RemoveRange(items);

            Assert.That(result, Is.False);
        }

        [Test]
        public void when_RemoveRangeAsync_is_called_then_Reset_event_fired()
        {
            var testSchedulerProvider = new TestDispatcherSchedulerProvider();

            var result = false;

            var bindableCollection = new BindableCollection<int>(testSchedulerProvider);

            var items = Enumerable.Range(0, 1).ToList();

            bindableCollection.AddRange(items);

            bindableCollection.CollectionChanged += (sender, args) =>
                                                    {
                                                        if (args.Action == NotifyCollectionChangedAction.Reset)
                                                        {
                                                            result = true;
                                                        }
                                                    };

            bindableCollection.RemoveRangeAsync(items);

            Assert.That(result, Is.True);
        }

        [Test]
        public void when_RemoveRangeAsync_is_called_and_IsNotifying_is_false_then_Reset_event_fired()
        {
            var testSchedulerProvider = new TestDispatcherSchedulerProvider();

            var result = false;

            var bindableCollection = new BindableCollection<int>(testSchedulerProvider);

            var items = Enumerable.Range(0, 1).ToList();

            bindableCollection.AddRange(items);

            bindableCollection.CollectionChanged += (sender, args) =>
                                                    {
                                                        if (args.Action == NotifyCollectionChangedAction.Reset)
                                                        {
                                                            result = true;
                                                        }
                                                    };

            bindableCollection.IsNotifying = false;

            bindableCollection.RemoveRangeAsync(items);

            Assert.That(result, Is.False);
        }

        [Test]
        public void when_Refresh_is_called_then_Reset_event_fired()
        {
            var testSchedulerProvider = new TestDispatcherSchedulerProvider();

            var result = false;

            var bindableCollection = new BindableCollection<int>(testSchedulerProvider);

            var items = Enumerable.Range(0, 1).ToList();

            bindableCollection.AddRange(items);

            bindableCollection.CollectionChanged += (sender, args) =>
                                                    {
                                                        if (args.Action == NotifyCollectionChangedAction.Reset)
                                                        {
                                                            result = true;
                                                        }
                                                    };

            bindableCollection.Refresh();

            Assert.That(result, Is.True);
        }

        [Test]
        public void when_Refresh_is_called_and_IsNotifying_is_false_then_Reset_event_fired()
        {
            var testSchedulerProvider = new TestDispatcherSchedulerProvider();

            var result = false;

            var bindableCollection = new BindableCollection<int>(testSchedulerProvider);

            var items = Enumerable.Range(0, 1).ToList();

            bindableCollection.AddRange(items);

            bindableCollection.CollectionChanged += (sender, args) =>
                                                    {
                                                        if (args.Action == NotifyCollectionChangedAction.Reset)
                                                        {
                                                            result = true;
                                                        }
                                                    };

            bindableCollection.IsNotifying = false;

            bindableCollection.Refresh();

            Assert.That(result, Is.False);
        }

        [Test]
        public void when_RefreshAsync_is_called_then_Reset_event_fired()
        {
            var testSchedulerProvider = new TestDispatcherSchedulerProvider();

            var result = false;

            var bindableCollection = new BindableCollection<int>(testSchedulerProvider);

            var items = Enumerable.Range(0, 1).ToList();

            bindableCollection.AddRange(items);

            bindableCollection.CollectionChanged += (sender, args) =>
                                                    {
                                                        if (args.Action == NotifyCollectionChangedAction.Reset)
                                                        {
                                                            result = true;
                                                        }
                                                    };

            bindableCollection.RefreshAsync();

            Assert.That(result, Is.True);
        }

        [Test]
        public void when_RefreshAsync_is_called_and_IsNotifying_is_false_then_Reset_event_fired()
        {
            var testSchedulerProvider = new TestDispatcherSchedulerProvider();

            var result = false;

            var bindableCollection = new BindableCollection<int>(testSchedulerProvider);

            var items = Enumerable.Range(0, 1).ToList();

            bindableCollection.AddRange(items);

            bindableCollection.CollectionChanged += (sender, args) =>
                                                    {
                                                        if (args.Action == NotifyCollectionChangedAction.Reset)
                                                        {
                                                            result = true;
                                                        }
                                                    };

            bindableCollection.IsNotifying = false;

            bindableCollection.RefreshAsync();

            Assert.That(result, Is.False);
        }

        [Test]
        public void when_items_are_added_those_items_are_pumped_onto_AddedItemsCollectionChanged()
        {
            var testSchedulerProvider = new TestDispatcherSchedulerProvider();

            var results = new List<Guid>();

            var bindableCollection = new BindableCollection<Guid>(testSchedulerProvider);
            bindableCollection.AddedItemsCollectionChanged
                              .Subscribe(x => results.AddRange(x));

            var items = Enumerable.Range(0, 10)
                                  .Select(_ => Guid.NewGuid())
                                  .ToList();

            bindableCollection.AddRange(items);

            var intersectionCount = results.Intersect(items).Count();

            Assert.That(intersectionCount, Is.EqualTo(items.Count));
        }
    }
}