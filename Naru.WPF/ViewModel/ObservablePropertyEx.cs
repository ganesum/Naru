using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reactive.Linq;

using FluentValidation;

using Naru.WPF.Scheduler;
using Naru.WPF.Validation;

namespace Naru.WPF.ViewModel
{
    public static class ObservablePropertyEx
    {
        public static TValue RaiseAndSetIfChanged<TValue>(this ObservableProperty<TValue> observableProperty,
                                                          TValue newValue)
        {
            // Inspired by ReactiveUI

            if (EqualityComparer<TValue>.Default.Equals(observableProperty.Value, newValue))
            {
                return newValue;
            }

            observableProperty.Value = newValue;

            return newValue;
        }

        public static IDisposable ConnectINPCProperty<T>(this ObservableProperty<T> observableProperty,
                                                         INotifyPropertyChangedEx viewModel,
                                                         Expression<Func<T>> propertyExpression,
                                                         ISchedulerProvider scheduler)
        {
            var propertyName = PropertyExtensions.ExtractPropertyName(propertyExpression);
            return observableProperty.ValueChanged
                                     .ObserveOn(scheduler.Dispatcher.RX)
                                     .Subscribe(x => viewModel.ConnectINPC(propertyName));
        }

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