using System.ComponentModel;

using FluentValidation;

namespace Naru.WPF.Validation
{
    public interface ISupportValidationAsync<T, TValidation> : INotifyDataErrorInfo
        where T : INotifyDataErrorInfo, ISupportValidationAsync<T, TValidation>
        where TValidation : AbstractValidator<T>, new()
    { }
}