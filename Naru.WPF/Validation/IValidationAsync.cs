using System;
using System.Collections;
using System.Linq.Expressions;
using System.Threading.Tasks;

using FluentValidation;

namespace Naru.WPF.Validation
{
    public interface IValidationAsync<T, TValidator>
        where T : ISupportValidationAsync<T, TValidator>
        where TValidator : AbstractValidator<T>, new()
    {
        IObservable<string> ErrorsChanged { get; }

        bool HasErrors { get; }

        void Initialise(T instance);

        IEnumerable GetErrors(string propertyName);

        Task<bool> IsValidAsync(T instance);

        void ValidateProperty<TProperty>(Expression<Func<TProperty>> propertyExpression);
    }
}