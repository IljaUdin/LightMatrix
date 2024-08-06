using Avalonia;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace LEDPanel_Avalonia.Converter
{
    public class NullToVisibilityConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter,
                                                                CultureInfo culture)
        {
            if (value != null)
            {
                return false; // Если значение не равно null, возвращаем false
            }
            else
            {
                return true; // Если значение равно null, возвращаем true
            }
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
