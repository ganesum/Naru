using System;
using System.Linq.Expressions;
using System.Reactive.Linq;

using FluentValidation;

using Naru.WPF.Scheduler;
using Naru.WPF.Validation;

namespace Naru.WPF.ViewModel
{
    public static class ObservablePropertyEx
    {
        public static IDisposable AddValidation<T, TModel, TValidation, TProperty>(this ObservableProperty<T> observableProperty,
                                                                                             ValidationAsync<TModel, TValidation> validation,
                                                                                             ISchedulerProvider scheduler,
                                                                                             Expression<Func<TProperty>> propertyExpression)
            where TModel : ISupportValidationAsync<TModel, TValidation>
            where TValidation : AbstractValidator<TModel>, new()
        {
            return observableProperty.ValueChanged
                                     .ObserveOn(scheduler.Dispatcher.RX)
                                     .Subscribe(_ => validation.ValidateProperty(propertyExpression));
        }
    }
}