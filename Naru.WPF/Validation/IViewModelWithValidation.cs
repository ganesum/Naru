using System.ComponentModel;

using FluentValidation;

namespace Naru.WPF.Validation
{
    public interface ISupportValidation<T, TValidation> : IDataErrorInfo
        where T : IDataErrorInfo
        where TValidation : AbstractValidator<T>, new()
    { }
}