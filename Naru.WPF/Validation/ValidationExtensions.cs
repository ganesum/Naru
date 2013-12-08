using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using FluentValidation.Results;

using Naru.TPL;
using Naru.WPF.Scheduler;

namespace Naru.WPF.Validation
{
    public static class ValidationExtensions
    {
        public static string GetError(this ValidationResult validationResults)
        {
            var validationErrors = new StringBuilder();

            foreach (var validationFailure in validationResults.Errors)
            {
                validationErrors.Append(validationFailure.ErrorMessage);
                validationErrors.Append(Environment.NewLine);
            }

            return validationErrors.ToString();
        }

        public static string GetErrorForProperty(this ValidationResult validationResults, string propertyName)
        {
            if (validationResults == null)
            {
                return string.Empty;
            }

            var columnResults = validationResults.Errors
                                                 .FirstOrDefault(x => string.Compare(x.PropertyName,
                                                                                     propertyName,
                                                                                     StringComparison.OrdinalIgnoreCase) == 0);
            
            return columnResults != null ? columnResults.ErrorMessage : string.Empty;
        }

        public static ValidationResult Validate<T, TValidation>(this T instance)
            where T : ISupportValidation<T, TValidation>
            where TValidation : AbstractValidator<T>, new()
        {
            IValidator<T> validator = new TValidation();
            return validator.Validate(instance);
        }

        public static Task<ValidationResult> ValidateAsync<T, TValidation>(this T instance,
                                                                           ISchedulerProvider schedulerProvider)
            where T : ISupportValidationAsync<T, TValidation>
            where TValidation : AbstractValidator<T>, new()
        {
            return Task.Factory.StartNew(() =>
                                         {
                                             IValidator<T> validator = new TValidation();
                                             return validator.Validate(instance);
                                         }, schedulerProvider.Task.TPL);
        }

        public static bool IsValid<T, TValidation>(this T instance)
            where T : ISupportValidation<T, TValidation>
            where TValidation : AbstractValidator<T>, new()
        {
            IValidator<T> validator = new TValidation();
            return validator.Validate(instance) != null;
        }

        public static Task<bool> IsValidAsync<T, TValidation>(this T instance, ISchedulerProvider schedulerProvider)
            where T : ISupportValidationAsync<T, TValidation>
            where TValidation : AbstractValidator<T>, new()
        {
            return Task.Factory.StartNew(() =>
                                         {
                                             IValidator<T> validator = new TValidation();
                                             var validationResult = validator.Validate(instance);
                                             return validationResult != null && !validationResult.Errors.Any();
                                         }, schedulerProvider.Task.TPL);
        }
    }
}