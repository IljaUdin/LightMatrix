using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace LEDPanel_Avalonia.Converter
{
    public class StringToFontFamilyConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string fontName)
            {
                return new FontFamily(fontName);
            }
            return FontFamily.Default;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is FontFamily fontFamily)
            {
                return fontFamily.Name;
            }
            return string.Empty;
        }
    }
}
