using System.ComponentModel;

using FluentValidation;

namespace Naru.WPF.Validation
{
    public interface ISupportValidation<T, TValidator> : IDataErrorInfo
        where T : IDataErrorInfo, ISupportValidation<T, TValidator>
        where TValidator : AbstractValidator<T>, new()
    { }
}