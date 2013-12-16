using System;
using System.Windows.Controls;

using Naru.WPF.MVVM;

using NUnit.Framework;

namespace Naru.WPF.Tests.MVVM
{
    [TestFixture]
    public class ContentControlViewResolverBehaviorTests
    {
        public class StubView : UserControl
        { }

        public class StubViewModel : Naru.WPF.ViewModel.ViewModel
        {
        }

        [Test]
        [STAThread]
        public void when_a_ContentControl_is_attached_and_its_DataContext_is_changed_to_a_ViewModel_then_the_correct_View_is_resoved()
        {
            var contentControl = new ContentControl();

            var behavior = new ContentControlViewResolverBehavior();

            var viewModel = new StubViewModel();

            behavior.Attach(contentControl);

            contentControl.DataContext = viewModel;

            var resolvedViewType = contentControl.Content.GetType();

            Assert.That(resolvedViewType.Equals(typeof(StubView)), Is.True);
        }
    }
}