using Avalonia.Data.Converters;
using System;
using System.Globalization;
using System.IO;

namespace LEDPanel_Avalonia.Converter
{
    public class FilePathToFileNameConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string filePath)
            {
                return Path.GetFileName(filePath); // Получаем только имя файла из полного пути
            }
            return value; // Возвращаем исходное значение, если не удалось преобразовать
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException(); // Этот конвертер не поддерживает обратное преобразование
        }
    }
}
