using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace LEDPanel_Avalonia.Converter
{
    internal class SolidColorBrushConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            //return new SolidColorBrush((Color)value);

            if (value is int intColor)
            {
                byte a = (byte)((intColor >> 24) & 0xFF);
                byte r = (byte)((intColor >> 16) & 0xFF);
                byte g = (byte)((intColor >> 8) & 0xFF);
                byte b = (byte)(intColor & 0xFF);

                return new SolidColorBrush(Color.FromArgb(a, r, g, b));
            }

            return new SolidColorBrush(Colors.Red);
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is Color color)
            {
                // Конвертируем Color в int ARGB
                int argb = (color.A << 24) | (color.R << 16) | (color.G << 8) | color.B;

                // Преобразуем int ARGB в HEX строку
                //string hexColor = $"#{argb:X8}"; // Форматирует как #AARRGGBB

                return argb;
            }

            return 0;
        }
    }

    internal class ColorConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int intColor)
            {
                byte a = (byte)((intColor >> 24) & 0xFF);
                byte r = (byte)((intColor >> 16) & 0xFF);
                byte g = (byte)((intColor >> 8) & 0xFF);
                byte b = (byte)(intColor & 0xFF);

                return Color.FromArgb(a, r, g, b);
            }

            return  Colors.Red;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is Color color)
            {
                // Конвертируем Color в int ARGB
                int argb = (color.A << 24) | (color.R << 16) | (color.G << 8) | color.B;

                // Преобразуем int ARGB в HEX строку
                //string hexColor = $"#{argb:X8}"; // Форматирует как #AARRGGBB

                return argb;
            }

            return 0;
        }
    }
}
