using System;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Reactive.Linq;

using FluentValidation;

using Naru.RX;
using Naru.WPF.Scheduler;
using Naru.WPF.Validation;

namespace Naru.WPF.ViewModel
{
    public static class ObservablePropertyEx
    {
        public static ObservableProperty<T> AddValidation<T, TModel, TValidation, TProperty>(this ObservableProperty<T> observableProperty,
                                                                                             ValidationAsync<TModel, TValidation> validation,
                                                                                             ISchedulerProvider scheduler,
                                                                                             Expression<Func<TProperty>> propertyExpression,
                                                                                             CompositeDisposable disposable)
            where TModel : ISupportValidationAsync<TModel, TValidation>
            where TValidation : AbstractValidator<TModel>, new()
        {
            observableProperty.ValueChanged
                              .ObserveOn(scheduler.Dispatcher.RX)
                              .Subscribe(_ => validation.ValidateProperty(propertyExpression))
                              .AddDisposable(disposable);

            return observableProperty;
        }
    }
}