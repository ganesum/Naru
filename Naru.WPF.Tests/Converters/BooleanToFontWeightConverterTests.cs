using System.Windows;

using Naru.WPF.Converters;

using NUnit.Framework;

namespace Naru.WPF.Tests.Converters
{
    [TestFixture]
    public class BooleanToFontWeightConverterTests
    {
        [Test]
        public void when_value_is_True_and_inverse_not_set_then_returns_Bold()
        {
            var converter = new BooleanToFontWeightConverter();
            var result = converter.Convert(true, null, null, null);

            Assert.That(result, Is.EqualTo(FontWeights.Bold));
        }

        [Test]
        public void when_value_is_True_and_inverse_is_set_then_returns_Bold()
        {
            var converter = new BooleanToFontWeightConverter();
            var result = converter.Convert(true, null, "inverse", null);

            Assert.That(result, Is.EqualTo(FontWeights.Normal));
        }

        [Test]
        public void when_value_is_False_and_inverse_not_set_then_returns_Bold()
        {
            var converter = new BooleanToFontWeightConverter();
            var result = converter.Convert(false, null, null, null);

            Assert.That(result, Is.EqualTo(FontWeights.Normal));
        }

        [Test]
        public void when_value_is_False_and_inverse_is_set_then_returns_Bold()
        {
            var converter = new BooleanToFontWeightConverter();
            var result = converter.Convert(false, null, "inverse", null);

            Assert.That(result, Is.EqualTo(FontWeights.Bold));
        }
    }
}