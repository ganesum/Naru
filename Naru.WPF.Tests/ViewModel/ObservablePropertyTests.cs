using System;

using Naru.WPF.ViewModel;

using NUnit.Framework;

namespace Naru.WPF.Tests.ViewModel
{
    [TestFixture]
    public class ObservablePropertyTests
    {
        [Test]
        public void when_Value_is_set_then_ValueChanged_pumps()
        {
            var expected = Guid.NewGuid().ToString();

            var result = string.Empty;

            var observableProperty = new ObservableProperty<string>();
            observableProperty.ValueChanged
                              .Subscribe(x => result = x);

            observableProperty.Value = expected;

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void when_initialValue_is_passed_to_the_constructor_then_Value_has_that_value()
        {
            var expected = Guid.NewGuid().ToString();

            var observableProperty = new ObservableProperty<string>(expected);

            Assert.That(observableProperty.Value, Is.EqualTo(expected));
        }
    }
}