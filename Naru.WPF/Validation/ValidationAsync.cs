using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

using FluentValidation;
using FluentValidation.Results;

using Naru.TPL;
using Naru.WPF.Scheduler;
using Naru.WPF.ViewModel;

namespace Naru.WPF.Validation
{
    public class ValidationAsync<T, TValidator> : IValidationAsync<T, TValidator>
        where T : ISupportValidationAsync<T, TValidator>
        where TValidator : AbstractValidator<T>, new()
    {
        private readonly ISchedulerProvider _scheduler;

        private readonly Dictionary<string, IEnumerable<string>> _validationErrors = new Dictionary<string, IEnumerable<string>>();

        private readonly Subject<string> _errorsChangedSubject = new Subject<string>();

        private readonly TValidator _validator;

        private T _instance;

        public IObservable<string> ErrorsChanged
        {
            get { return _errorsChangedSubject.AsObservable(); }
        }

        public ValidationAsync(ISchedulerProvider scheduler)
        {
            _scheduler = scheduler;

            _validator = new TValidator();
        }

        public void Initialise(T instance)
        {
            _instance = instance;
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !_validationErrors.ContainsKey(propertyName))
            {
                return null;
            }

            return _validationErrors[propertyName];
        }

        public bool HasErrors
        {
            get { return _validationErrors.Any(x => x.Value.Any(error => !string.IsNullOrEmpty(error))); }
        }

        public Task<bool> IsValidAsync(T instance)
        {
            return Task.Factory.StartNew(() =>
                                         {
                                             IValidator<T> validator = new TValidator();
                                             var validationResult = validator.Validate(instance);
                                             return validationResult != null && !validationResult.Errors.Any();
                                         }, _scheduler.Task.TPL);
        }

        public void ValidateProperty<TProperty>(Expression<Func<TProperty>> propertyExpression)
        {
            var propertyName = PropertyExtensions.ExtractPropertyName(propertyExpression);

            ValidateAsync()
                .Then(validationResults =>
                      {
                          var propertyErrors = validationResults.GetErrorForProperty(propertyName)
                                                                .Split('\n');

                          return ProcessValidationResults(propertyName, propertyErrors);
                      }, _scheduler.Dispatcher.TPL)
                .CatchAndHandle(exc =>
                                {
                                    var errorMessage = string.Format("Error during validation : {0}", exc.Message);
                                    _validationErrors[propertyName] = new[] {errorMessage};

                                    RaiseErrorsChanged(propertyName);
                                }, _scheduler.Dispatcher.TPL);
        }

        private Task ProcessValidationResults(string propertyName, IEnumerable<string> validationErrors)
        {
            return Task.Factory
                       .StartNew(() =>
                                 {
                                     if (validationErrors.Any(x => !string.IsNullOrEmpty(x)))
                                     {
                                         _validationErrors[propertyName] = validationErrors;

                                         RaiseErrorsChanged(propertyName);
                                     }
                                     else if (_validationErrors.ContainsKey(propertyName))
                                     {
                                         _validationErrors.Remove(propertyName);

                                         RaiseErrorsChanged(propertyName);
                                     }
                                 },
                                 _scheduler.Dispatcher.TPL);
        }

        private void RaiseErrorsChanged(string propertyName)
        {
            _errorsChangedSubject.OnNext(propertyName);
        }

        private Task<ValidationResult> ValidateAsync()
        {
            return Task.Factory.StartNew(() => _validator.Validate(_instance), _scheduler.Task.TPL);
        }
    }
}