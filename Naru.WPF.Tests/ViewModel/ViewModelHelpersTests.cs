using Common.Logging;
using Common.Logging.Simple;

using Naru.Tests;
using Naru.WPF.Dialog;
using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;
using Naru.WPF.Tests.Scheduler;
using Naru.WPF.ToolBar;
using Naru.WPF.ViewModel;

using NUnit.Framework;

namespace Naru.WPF.Tests.ViewModel
{
    [TestFixture]
    public class ViewModelHelpersTests
    {
        public class WorkspaceViewModel : Workspace
        {
            public WorkspaceViewModel(ILog log, IDispatcherSchedulerProvider scheduler, IStandardDialog standardDialog) 
                : base(log, scheduler, standardDialog)
            {
            }
        }

        [Test]
        public void when_parent_is_activated_child_is_activated()
        {
            var container = AutoMock.GetLoose();

            container.Provide<IDispatcherSchedulerProvider>(new TestDispatcherSchedulerProvider());

            var parent = container.Create<WorkspaceViewModel>() as ISupportActivationState;
            parent.ActivationStateViewModel.DeActivate();
            Assert.That(parent.ActivationStateViewModel.IsActive, Is.False);

            var child = container.Create<WorkspaceViewModel>() as ISupportActivationState;
            child.ActivationStateViewModel.DeActivate();
            Assert.That(child.ActivationStateViewModel.IsActive, Is.False);

            parent.SyncViewModelActivationStates(child);

            parent.ActivationStateViewModel.Activate();

            Assert.That(parent.ActivationStateViewModel.IsActive, Is.True);
            Assert.That(child.ActivationStateViewModel.IsActive, Is.True);
        }

        [Test]
        public void when_parent_is_deactivated_child_is_activated()
        {
            var container = AutoMock.GetLoose();

            container.Provide<IDispatcherSchedulerProvider>(new TestDispatcherSchedulerProvider());

            var parent = container.Create<WorkspaceViewModel>() as ISupportActivationState;
            parent.ActivationStateViewModel.Activate();
            Assert.That(parent.ActivationStateViewModel.IsActive, Is.True);

            var child = container.Create<WorkspaceViewModel>() as ISupportActivationState;
            child.ActivationStateViewModel.Activate();
            Assert.That(child.ActivationStateViewModel.IsActive, Is.True);

            parent.SyncViewModelActivationStates(child);

            parent.ActivationStateViewModel.DeActivate();

            Assert.That(parent.ActivationStateViewModel.IsActive, Is.False);
            Assert.That(child.ActivationStateViewModel.IsActive, Is.False);
        }

        [Test]
        public void when_header_is_created_the_properties_have_the_correct_values()
        {
            var container = AutoMock.GetLoose();

            var testDispatcherSchedulerProvider = new TestDispatcherSchedulerProvider();
            container.Provide<IDispatcherSchedulerProvider>(testDispatcherSchedulerProvider);

            var workspace = container.Create<WorkspaceViewModel>();

            var displayName = "DisplayName";
            var imageuri = "ImageUri";

            workspace.SetupHeader(testDispatcherSchedulerProvider, displayName, imageuri);

            var headerViewModel = workspace.Header as HeaderViewModel;

            Assert.That(headerViewModel, Is.Not.Null);
            Assert.That(headerViewModel.DisplayName, Is.EqualTo(displayName));
            Assert.That(headerViewModel.ImageName, Is.EqualTo(imageuri));
        }

        public class SupportBusyViewModel : ISupportBusy
        {
            public IBusyViewModel BusyViewModel { get; private set; }

            public SupportBusyViewModel(IDispatcherSchedulerProvider scheduler)
            {
                BusyViewModel = new BusyViewModel(scheduler);
            }
        }

        [Test]
        public void when_child_isbusy_changes_then_the_parent_isbusy_changes()
        {
            var testSchedulerProvider = new TestDispatcherSchedulerProvider();

            var parent = new SupportBusyViewModel(testSchedulerProvider);
            var child = new SupportBusyViewModel(testSchedulerProvider);

            parent.SyncViewModelBusy(child);

            Assert.That(parent.BusyViewModel.IsActive, Is.False);
            Assert.That(child.BusyViewModel.IsActive, Is.False);

            child.BusyViewModel.Active(string.Empty);

            Assert.That(child.BusyViewModel.IsActive, Is.True);

            Assert.That(parent.BusyViewModel.IsActive, Is.True);

            child.BusyViewModel.InActive();

            Assert.That(child.BusyViewModel.IsActive, Is.False);

            Assert.That(parent.BusyViewModel.IsActive, Is.False);
        }

        public class SupportActivationState : ISupportActivationState
        {
            public IActivationStateViewModel ActivationStateViewModel { get; private set; }

            public SupportActivationState(IDispatcherSchedulerProvider scheduler)
            {
                ActivationStateViewModel = new ActivationStateViewModel(new NoOpLogger(), scheduler);
            }
        }

        [Test]
        public void when_parent_activation_state_changes_then_the_toolbaritem_activate_state_changes()
        {
            var testSchedulerProvider = new TestDispatcherSchedulerProvider();

            var toolBarItem1 = new ToolBarButtonItem(testSchedulerProvider);
            var toolBarItem2 = new ToolBarButtonItem(testSchedulerProvider);

            var viewModel = new SupportActivationState(testSchedulerProvider);
            viewModel.ActivationStateViewModel.Activate();

            viewModel.SyncToolBarItemWithViewModelActivationState(toolBarItem1, toolBarItem2);

            Assert.That(viewModel.ActivationStateViewModel.IsActive, Is.True);

            Assert.That(toolBarItem1.IsVisible, Is.True);
            Assert.That(toolBarItem2.IsVisible, Is.True);

            viewModel.ActivationStateViewModel.DeActivate();

            Assert.That(viewModel.ActivationStateViewModel.IsActive, Is.False);

            Assert.That(toolBarItem1.IsVisible, Is.False);
            Assert.That(toolBarItem2.IsVisible, Is.False);

            viewModel.ActivationStateViewModel.Activate();

            Assert.That(viewModel.ActivationStateViewModel.IsActive, Is.True);

            Assert.That(toolBarItem1.IsVisible, Is.True);
            Assert.That(toolBarItem2.IsVisible, Is.True);
        }

        public class SupportClosing : ISupportClosing
        {
            public IClosingStrategy ClosingStrategy { get; private set; }

            public SupportClosing()
            {
                ClosingStrategy = new ClosingStrategy(new NoOpLogger());
            }
        }

        [Test]
        public void when_closed_action_is_executed()
        {
            var supportClose = new SupportClosing();

            var result = false;

            supportClose.ExecuteOnClosed(() => result = true);

            supportClose.ClosingStrategy.Close();

            Assert.That(result, Is.True);
        }

        [Test]
        public void when_parent_closes_then_the_child_closes()
        {
            var parent = new SupportClosing();

            var child = new SupportClosing();

            var result = false;

            child.ExecuteOnClosed(() => result = true);

            child.SyncViewModelClose(parent);

            parent.ClosingStrategy.Close();

            Assert.That(result, Is.True);
        }
    }
}