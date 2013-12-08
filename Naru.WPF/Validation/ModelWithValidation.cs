using System;

using FluentValidation;

namespace Naru.WPF.Validation
{
    public abstract class ModelWithValidation<TModel, TValidation> : ViewModel.ViewModel,
                                                                     ISupportValidation<TModel, TValidation>,
                                                                     IDisposable
        where TModel : ModelWithValidation<TModel, TValidation>
        where TValidation : AbstractValidator<TModel>, new()
    {
        #region IDataErrorInfo Members

        public string Error
        {
            get
            {
                return ((TModel)this).Validate<TModel, TValidation>().GetError();
            }
        }

        public string this[string columnName]
        {
            get
            {
                return ((TModel)this).Validate<TModel, TValidation>().GetErrorForProperty(columnName);
            }
        }

        #endregion

        public void Dispose()
        {
            Disposables.Dispose();
        }
    }
}