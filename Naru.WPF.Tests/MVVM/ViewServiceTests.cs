using System.Windows.Controls;

using Common.Logging;

using Naru.Tests.UnityAutoMockContainer;
using Naru.WPF.MVVM;

using NUnit.Framework;

namespace Naru.WPF.Tests.MVVM
{
    [TestFixture]
    public class ViewServiceTests
    {
        public class StubView : UserControl
        { }

        public class StubViewModel : Workspace
        {
            public StubViewModel(ILog log, IDispatcherService dispatcherService) 
                : base(log, dispatcherService)
            {
            }
        }

        public class StubViewNameNotMatchingView : UserControl
        { }

        [UseView(typeof(StubViewNameNotMatchingView))]
        public class StubViewModelWithUseViewAttribute : Workspace
        {
            public StubViewModelWithUseViewAttribute(ILog log, IDispatcherService dispatcherService)
                : base(log, dispatcherService)
            {
            }
        }

        [Test]
        public void check_viewmodel_is_set_as_the_datacontext_of_the_view()
        {
            var container = new UnityAutoMockContainer();

            var view = new UserControl();
            Assert.That(view.DataContext, Is.Null);

            var viewModel = container.Resolve<StubViewModel>();

            ViewService.BindViewModel(view, viewModel);

            Assert.That(view.DataContext, Is.EqualTo(viewModel));
        }

        [Test]
        public void check_that_the_correct_view_is_resolved_from_the_viewmodel_convention()
        {
            var container = new UnityAutoMockContainer();

            var viewModel = container.Resolve<StubViewModel>();

            var view = ViewService.CreateView(viewModel.GetType());

            Assert.That(view.GetType(), Is.EqualTo(typeof(StubView)));
        }

        [Test]
        public void check_that_the_correct_view_is_resolved_from_the_viewmodel_UseViewAttribute()
        {
            var container = new UnityAutoMockContainer();

            var viewModel = container.Resolve<StubViewModelWithUseViewAttribute>();

            var view = ViewService.CreateView(viewModel.GetType());

            Assert.That(view.GetType(), Is.EqualTo(typeof(StubViewNameNotMatchingView)));
        }
    }
}