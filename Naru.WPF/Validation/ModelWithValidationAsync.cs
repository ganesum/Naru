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
        private readonly ISchedulerProvider _scheduler;
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

        public IObservable<bool> IsValid
        {
            get
            {
                return Validation.ErrorsChanged
                    .SelectMany(_ => ((TModel)this).IsValidAsync<TModel, TValidation>(_scheduler))
                    .DistinctUntilChanged();
            }
        }

        protected ModelWithValidationAsync(ISchedulerProvider scheduler, ValidationAsync<TModel, TValidation> validation)
        {
            _scheduler = scheduler;

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