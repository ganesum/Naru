﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Naru.WPF.MVVM
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isVisible;

            var result = Visibility.Collapsed;

            if (bool.TryParse(value.ToString(), out isVisible))
                if (isVisible)
                    result = Visibility.Visible;

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visibility = (Visibility) value;

            return visibility == Visibility.Visible;
        }
    }
}