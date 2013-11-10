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

        public static ValidationResult Validate<TValidation, T>(this T model)
            where T : ISupportValidation<T, TValidation>
            where TValidation : AbstractValidator<T>, new()
        {
            IValidator<T> validator = new TValidation();
            return validator.Validate(model);
        }

        public static Task<ValidationResult> ValidateAsync<TValidation, T>(this T model,
                                                                           ISchedulerProvider schedulerProvider)
            where T : ISupportValidation<T, TValidation>
            where TValidation : AbstractValidator<T>, new()
        {
            return Task.Factory.StartNew(() => Validate<TValidation, T>(model), schedulerProvider.TPL.Task);
        }

        public static bool IsValid<TValidation, T>(this T model)
            where T : ISupportValidation<T, TValidation>
            where TValidation : AbstractValidator<T>, new()
        {
            IValidator<T> validator = new TValidation();
            return validator.Validate(model) != null;
        }

        public static Task<bool> IsValidAsync<TValidation, T>(this T model, ISchedulerProvider schedulerProvider)
            where T : ISupportValidation<T, TValidation>
            where TValidation : AbstractValidator<T>, new()
        {
            return Task.Factory.StartNew(() => IsValid<TValidation, T>(model), schedulerProvider.TPL.Task);
        }
    }
}