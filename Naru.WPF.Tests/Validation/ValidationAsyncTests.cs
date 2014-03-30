using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

using FluentValidation;

using Microsoft.Reactive.Testing;

using Naru.WPF.Tests.Scheduler;
using Naru.WPF.Validation;

using NUnit.Framework;

namespace Naru.WPF.Tests.Validation
{
    [TestFixture]
    public class ValidationAsyncTests
    {
        public class ErrorValidatingObjectValidator : AbstractValidator<ErrorValidatingObject>
        {
            public const string TestException = "Test Exception";

            public ErrorValidatingObjectValidator()
            {
                RuleFor(x => x.ErrorValidating).Must(_ => { throw new Exception(TestException); });
            }
        }

        public class ErrorValidatingObject : ISupportValidationAsync<ErrorValidatingObject, ErrorValidatingObjectValidator>
        {
            public string ErrorValidating { get; set; }

            public IEnumerable GetErrors(string propertyName)
            {
                throw new NotImplementedException();
            }

            public bool HasErrors { get; private set; }

            public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        }

        public class ValidatingObjectValidator : AbstractValidator<ValidatingObject>
        {
            public ValidatingObjectValidator()
            {
                RuleFor(x => x.Name).NotNull();
            }
        }

        public class ValidatingObject : ISupportValidationAsync<ValidatingObject, ValidatingObjectValidator>
        {
            public string Name { get; set; }

            public IEnumerable GetErrors(string propertyName)
            {
                throw new NotImplementedException();
            }

            public bool HasErrors { get; private set; }

            public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        }

        [Test]
        public void when_ValidateProperty_is_called_then_ErrorsChanged_pumps()
        {
            var testSchedulerProvider = new TestDispatcherSchedulerProvider();
            var rxTestScheduler = testSchedulerProvider.Current.RX as TestScheduler;

            var errorChangedPumped = false;

            var validatingObject = new ValidatingObject();

            var sut = new ValidationAsync<ValidatingObject, ValidatingObjectValidator>(testSchedulerProvider);
            sut.Initialise(validatingObject);

            sut.ErrorsChanged
               .ObserveOn(rxTestScheduler)
               .Subscribe(_ => errorChangedPumped = true);

            Assert.That(errorChangedPumped, Is.False);

            sut.ValidateProperty(() => validatingObject.Name);

            rxTestScheduler.AdvanceBy(1);

            Assert.That(errorChangedPumped, Is.True);
        }

        [Test]
        public void when_a_property_is_invalid_then_HasErrors_return_True()
        {
            var testSchedulerProvider = new TestDispatcherSchedulerProvider();

            var validatingObject = new ValidatingObject();

            var sut = new ValidationAsync<ValidatingObject, ValidatingObjectValidator>(testSchedulerProvider);
            sut.Initialise(validatingObject);

            sut.ValidateProperty(() => validatingObject.Name);

            Assert.That(sut.HasErrors, Is.True);
        }

        [Test]
        public void when_a_property_is_invalid_then_GetErrors_return_the_correct_validation_results()
        {
            var testSchedulerProvider = new TestDispatcherSchedulerProvider();

            var validatingObject = new ValidatingObject();

            var sut = new ValidationAsync<ValidatingObject, ValidatingObjectValidator>(testSchedulerProvider);
            sut.Initialise(validatingObject);

            sut.ValidateProperty(() => validatingObject.Name);
            
            var validationResults = sut.GetErrors("Name").OfType<string>().ToList();

            Assert.That(validationResults.Count(), Is.EqualTo(1));
            Assert.That(validationResults[0], Is.EqualTo("'Name' must not be empty."));
        }

        [Test]
        public void when_there_is_an_exception_during_validation_then_the_exception_is_diplayed_in_the_valiation_result()
        {
            var testSchedulerProvider = new TestDispatcherSchedulerProvider();

            var validatingObject = new ErrorValidatingObject();

            var sut = new ValidationAsync<ErrorValidatingObject, ErrorValidatingObjectValidator>(testSchedulerProvider);
            sut.Initialise(validatingObject);

            sut.ValidateProperty(() => validatingObject.ErrorValidating);

            var validationResults = sut.GetErrors("ErrorValidating").OfType<string>().ToList();

            Assert.That(validationResults.Count(), Is.EqualTo(1));
            Assert.That(validationResults[0], Is.EqualTo(string.Format("Error during validation : {0}", ErrorValidatingObjectValidator.TestException)));
        }
    }
}