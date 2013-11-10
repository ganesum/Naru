using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

using FluentValidation;

using Naru.Core;
using Naru.TPL;
using Naru.WPF.Scheduler;
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

    public abstract class ModelWithValidationAsync<TModel, TValidation> : NotifyPropertyChanged,
                                                                          ISupportValidationAsync<TModel, TValidation>
        where TModel : ModelWithValidationAsync<TModel, TValidation>
        where TValidation : AbstractValidator<TModel>, new()
    {
        private readonly ISchedulerProvider _scheduler;

        private readonly Dictionary<string, IEnumerable<string>> _validationErrors =
            new Dictionary<string, IEnumerable<string>>();

        protected ModelWithValidationAsync(ISchedulerProvider scheduler)
        {
            _scheduler = scheduler;
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !_validationErrors.ContainsKey(propertyName))
            {
                return null;
            }

            return _validationErrors[propertyName];
        }

        public bool HasErrors
        {
            get { return _validationErrors.Count > 0; }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged.SafeInvoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private Task ProcessValidationResults(string propertyName, bool isValid, IEnumerable<string> validationErrors)
        {
            return Task.Factory
                       .StartNew(() =>
                                 {
                                     if (!isValid)
                                     {
                                         _validationErrors[propertyName] = validationErrors;
                                         RaiseErrorsChanged(propertyName);
                                     }
                                     else if (_validationErrors.ContainsKey(propertyName))
                                     {
                                         _validationErrors.Remove(propertyName);
                                         RaiseErrorsChanged(propertyName);
                                     }
                                 },
                                 _scheduler.TPL.Dispatcher);
        }
    }
}