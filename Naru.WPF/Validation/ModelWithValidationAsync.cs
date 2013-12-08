using System;
using System.Collections;
using System.ComponentModel;
using System.Reactive.Linq;

using FluentValidation;

using Naru.Core;
using Naru.RX;
using Naru.WPF.Scheduler;

namespace Naru.WPF.Validation
{
    public abstract class ModelWithValidationAsync<TModel, TValidation> : ViewModel.ViewModel,
                                                                          ISupportValidationAsync<TModel, TValidation>,
                                                                          IDisposable
        where TModel : ModelWithValidationAsync<TModel, TValidation>
        where TValidation : AbstractValidator<TModel>, new()
    {
        protected readonly ValidationAsync<TModel, TValidation> Validation;

        #region INotifyDataErrorInfo

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return Validation.GetErrors(propertyName);
        }

        public bool HasErrors
        {
            get { return Validation.HasErrors; }
        }

        #endregion

        protected ModelWithValidationAsync(ISchedulerProvider scheduler, ValidationAsync<TModel, TValidation> validation)
        {
            Validation = validation;
            Validation.Initialise((TModel) this);
            Validation.ErrorsChanged
                       .ObserveOn(scheduler.Dispatcher.RX)
                       .Subscribe(x => ErrorsChanged.SafeInvoke(this, new DataErrorsChangedEventArgs(x)))
                       .AddDisposable(Disposables);
        }

        public void Dispose()
        {
            Disposables.Dispose();
        }
    }
}