using System;

using Microsoft.Reactive.Testing;

using Naru.RX;
using Naru.WPF.Scheduler;
using Naru.WPF.Tests.Scheduler;
using Naru.WPF.ViewModel;

using NUnit.Framework;

namespace Naru.WPF.Tests.ViewModel
{
    [TestFixture]
    public class ObservablePropertyExTests
    {
        public class TestViewModel : Naru.WPF.ViewModel.ViewModel
        {
            #region Name

            private readonly ObservableProperty<string> _name = new ObservableProperty<string>();

            public string Name
            {
                get { return _name.Value; }
                set { _name.RaiseAndSetIfChanged(value); }
            }

            #endregion

            public TestViewModel(ISchedulerProvider scheduler)
            {
                _name.ConnectINPCProperty(this, () => Name, scheduler).AddDisposable(Disposables);
            }
        }

        [Test]
        public void when_RaiseAndSetIfChanged_is_called_with_a_different_value_then_it_is_set()
        {
            var testSchedulerProvider = new TestSchedulerProvider();

            var viewModel = new TestViewModel(testSchedulerProvider);

            Assert.That(viewModel.Name, Is.Null);

            var value1 = Guid.NewGuid().ToString();
            viewModel.Name = value1;

            Assert.That(viewModel.Name, Is.EqualTo(value1));

            var value2 = Guid.NewGuid().ToString();
            viewModel.Name = value2;

            Assert.That(viewModel.Name, Is.EqualTo(value2));
        }

        [Test]
        public void when_ConnectINPCProperty_is_setup_then_when_ObservableProperty_changes_then_INPC_is_fired()
        {
            var testSchedulerProvider = new TestSchedulerProvider();

            var viewModel = new TestViewModel(testSchedulerProvider);

            var result = false;

            viewModel.PropertyChanged += (sender, args) =>
                                         {
                                             var propertyName = PropertyExtensions.ExtractPropertyName(() => viewModel.Name);
                                             if (args.PropertyName == propertyName)
                                             {
                                                 result = true;
                                             }
                                         };

            viewModel.Name = Guid.NewGuid().ToString();

            ((TestScheduler)testSchedulerProvider.Dispatcher.RX).AdvanceBy(1);

            Assert.That(result, Is.True);
        }
    }
}