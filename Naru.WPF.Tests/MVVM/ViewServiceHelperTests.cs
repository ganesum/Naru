using System;
using System.Windows.Controls;

using Common.Logging;

using Naru.Concurrency.Scheduler;
using Naru.Tests;
using Naru.WPF.Dialog;
using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;
using Naru.WPF.Tests.Scheduler;
using Naru.WPF.ViewModel;

using NUnit.Framework;

namespace Naru.WPF.Tests.MVVM
{
    [TestFixture]
    public class ViewServiceHelperTests
    {
        public class StubView : UserControl
        { }

        public class StubViewModel : Workspace
        {
            public StubViewModel(ILog log, IDispatcherSchedulerProvider scheduler, IStandardDialog standardDialog) 
                : base(log, scheduler, standardDialog)
            {
            }
        }

        public class StubViewNameNotMatchingView : UserControl
        { }

        [UseView(typeof(StubViewNameNotMatchingView))]
        public class StubViewModelWithUseViewAttribute : Workspace
        {
            public StubViewModelWithUseViewAttribute(ILog log, IDispatcherSchedulerProvider scheduler, IStandardDialog standardDialog)
                : base(log, scheduler, standardDialog)
            {
            }
        }

        [Test]
        [STAThread]
        public void check_viewmodel_is_set_as_the_datacontext_of_the_view()
        {
            var container = AutoMock.GetStrict();

            container.Provide<ISchedulerProvider>(new TestDispatcherSchedulerProvider());

            var view = new UserControl();
            Assert.That(view.DataContext, Is.Null);

            var viewModel = container.Create<StubViewModel>();

            ViewServiceHelper.BindViewModel(view, viewModel);

            Assert.That(view.DataContext, Is.EqualTo(viewModel));
        }

        [Test]
        [STAThread]
        public void check_that_the_correct_view_is_resolved_from_the_viewmodel_convention()
        {
            var container = AutoMock.GetStrict();

            container.Provide<ISchedulerProvider>(new TestDispatcherSchedulerProvider());

            var viewModel = container.Create<StubViewModel>();

            var view = ViewServiceHelper.CreateView(viewModel.GetType());

            Assert.That(view.GetType(), Is.EqualTo(typeof(StubView)));
        }

        [Test]
        [STAThread]
        public void check_that_the_correct_view_is_resolved_from_the_viewmodel_UseViewAttribute()
        {
            var container = AutoMock.GetStrict();

            container.Provide<ISchedulerProvider>(new TestDispatcherSchedulerProvider());

            var viewModel = container.Create<StubViewModelWithUseViewAttribute>();

            var view = ViewServiceHelper.CreateView(viewModel.GetType());

            Assert.That(view.GetType(), Is.EqualTo(typeof(StubViewNameNotMatchingView)));
        }
    }
}