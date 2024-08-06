using Avalonia.Controls;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LEDPanel_Avalonia.Converter
{
    public class CheckBoxToGridLinesVisibilityConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isChecked)
            {
                return isChecked ? DataGridGridLinesVisibility.All : DataGridGridLinesVisibility.None;
            }

            return DataGridGridLinesVisibility.None;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DataGridGridLinesVisibility visibility)
            {
                return visibility == DataGridGridLinesVisibility.All;
            }

            return false;
        }
    }
}
