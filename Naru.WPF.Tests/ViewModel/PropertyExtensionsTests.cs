using Naru.WPF.ViewModel;

using NUnit.Framework;

namespace Naru.WPF.Tests.ViewModel
{
    [TestFixture]
    public class PropertyExtensionsTests
    {
        [Test]
        public void resolves_the_correct_property_name_1()
        {
            var instance = new TestPropertyClass();

            var result = PropertyExtensions.ExtractPropertyName(() => instance.Property1);

            var expected = "Property1";

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void resolves_the_correct_property_name_2()
        {
            var instance = new TestPropertyClass();

            var result = PropertyExtensions.ExtractPropertyName(() => instance.Property2);

            var expected = "Property2";

            Assert.That(result, Is.EqualTo(expected));
        }

        public class TestPropertyClass
        {
            public string Property1 { get; set; }

            public int Property2 { get; set; }
        }
    }
}