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
    public abstract class ModelWithValidationAsync<TModel, TValidator> : ViewModel.ViewModel,
                                                                         ISupportValidationAsync<TModel, TValidator>,
                                                                         IDisposable
        where TModel : ModelWithValidationAsync<TModel, TValidator>
        where TValidator : AbstractValidator<TModel>, new()
    {
        protected readonly ValidationAsync<TModel, TValidator> Validation;

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

        public IObservable<bool> IsValid
        {
            get
            {
                return Validation.ErrorsChanged
                                 .SelectMany(_ => Validation.IsValidAsync((TModel) this))
                                 .DistinctUntilChanged();
            }
        }

        protected ModelWithValidationAsync(ISchedulerProvider scheduler, ValidationAsync<TModel, TValidator> validation)
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