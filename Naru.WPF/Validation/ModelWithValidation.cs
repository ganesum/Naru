using FluentValidation;

using Naru.WPF.ViewModel;

namespace Naru.WPF.Validation
{
    public abstract class ModelWithValidation<TModel, TValidation> : NotifyPropertyChanged,
                                                                     ISupportValidation<TModel, TValidation>
        where TModel : ModelWithValidation<TModel, TValidation>
        where TValidation : AbstractValidator<TModel>, new()
    {
        #region IDataErrorInfo Members

        public string Error
        {
            get
            {
                return ((TModel) this).Validate<TValidation, TModel>().GetError();
            }
        }

        public string this[string columnName]
        {
            get
            {
                return ((TModel) this).Validate<TValidation, TModel>().GetErrorForProperty(columnName);
            }
        }

        #endregion
    }
}