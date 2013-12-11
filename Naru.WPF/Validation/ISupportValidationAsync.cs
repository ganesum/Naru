using System.ComponentModel;

using FluentValidation;

namespace Naru.WPF.Validation
{
    public interface ISupportValidationAsync<T, TValidator> : INotifyDataErrorInfo
        where T : INotifyDataErrorInfo, ISupportValidationAsync<T, TValidator>
        where TValidator : AbstractValidator<T>, new()
    { }
}