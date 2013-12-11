using System;
using System.Linq;
using System.Text;

using FluentValidation.Results;

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
    }
}