using System.ComponentModel;

using FluentValidation;

namespace Naru.WPF.Validation
{
    public interface ISupportValidation<T, TValidation> : IDataErrorInfo
        where T : IDataErrorInfo, ISupportValidation<T, TValidation>
        where TValidation : AbstractValidator<T>, new()
    { }
}