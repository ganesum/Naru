using FluentValidation;
using FluentValidation.Results;

namespace Naru.WPF.Validation
{
    public abstract class ModelWithValidation<TModel, TValidator> : ISupportValidation<TModel, TValidator>
        where TModel : ModelWithValidation<TModel, TValidator>
        where TValidator : AbstractValidator<TModel>, new()
    {
        private readonly TValidator _validator;

        #region IDataErrorInfo Members

        public string Error
        {
            get
            {
                return Validate().GetError();
            }
        }

        public string this[string columnName]
        {
            get
            {
                return Validate().GetErrorForProperty(columnName);
            }
        }

        #endregion

        protected ModelWithValidation()
        {
            _validator = new TValidator();
        }

        public ValidationResult Validate()
        {
            return _validator.Validate((TModel) this);
        }

        public bool IsValid()
        {
            return _validator.Validate((TModel)this) != null;
        }
    }
}