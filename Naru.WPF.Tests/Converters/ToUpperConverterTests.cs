using Naru.WPF.Converters;

using NUnit.Framework;

namespace Naru.WPF.Tests.Converters
{
    [TestFixture]
    public class ToUpperConverterTests
    {
        [Test]
        public void when_all_lower_case_is_passed_in_then_all_is_converted_to_uppercase()
        {
            var input = "abcdefg";

            var converter = new ToUpperConverter();
            var result = converter.Convert(input, null, null, null);

            var expected = input.ToUpper();

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void when_mixture_of_lower_and_upper_case_is_passed_in_then_all_is_converted_to_uppercase()
        {
            var input1 = "abcdefg";
            var input2 = "abcdefg";

            var converter = new ToUpperConverter();
            var result = converter.Convert(string.Format("{0}{1}", input1, input2.ToUpper()), null, null, null);

            var expected = string.Format("{0}{1}", input1, input2).ToUpper();

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void when_all_upper_case_is_passed_in_then_remains_the_same()
        {
            var input = "abcdefg".ToUpper();

            var converter = new ToUpperConverter();
            var result = converter.Convert(input, null, null, null);

            var expected = input.ToUpper();

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}