﻿using System;
using Windows.UI.Xaml.Data;

namespace RybinskTaxi.Common
{
    /// <summary>
    /// Конвертер значений, который преобразует значение true в значение false и наоборот.
    /// </summary>
    public sealed class BooleanNegationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return !(value is bool && (bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return !(value is bool && (bool)value);
        }
    }
}
