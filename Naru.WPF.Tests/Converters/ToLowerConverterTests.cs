using Naru.WPF.Converters;

using NUnit.Framework;

namespace Naru.WPF.Tests.Converters
{
    [TestFixture]
    public class ToLowerConverterTests
    {
        [Test]
        public void when_all_upper_case_is_passed_in_then_all_is_converted_to_lowercase()
        {
            var input = "ABCDEFG";

            var converter = new ToLowerConverter();
            var result = converter.Convert(input, null, null, null);

            var expected = input.ToLower();

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void when_mixture_of_lower_and_upper_case_is_passed_in_then_all_is_converted_to_lowercase()
        {
            var input1 = "ABCDEFG";
            var input2 = "ABCDEFG";

            var converter = new ToLowerConverter();
            var result = converter.Convert(string.Format("{0}{1}", input1, input2.ToLower()), null, null, null);

            var expected = string.Format("{0}{1}", input1, input2).ToLower();

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void when_all_lower_case_is_passed_in_then_remains_the_same()
        {
            var input = "ABCDEFG".ToLower();

            var converter = new ToLowerConverter();
            var result = converter.Convert(input, null, null, null);

            var expected = input.ToLower();

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}