using Avalonia.Data.Converters;
using Avalonia;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LEDPanel_Avalonia.Converter
{
    internal class LocationCellConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int[] array && array.Length == 4)
            {
                return new Thickness(array[0], array[1], array[2], array[3]);
            }

            throw new ArgumentException("Expected value to be a int array of length 4.");
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is Thickness thickness)
            {
                return new double[] { thickness.Left, thickness.Top, thickness.Right, thickness.Bottom };
            }

            throw new ArgumentException("Expected value to be a Thickness.");
        }
    }
}
