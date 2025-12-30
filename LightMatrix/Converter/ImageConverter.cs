using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightMatrix.Converter
{
    public class ImageConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string path && !string.IsNullOrWhiteSpace(path))
            {
                try
                {
                    var imageExtensions = new[] { ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".ico" };
                    
                    bool isImage = imageExtensions.Contains(Path.GetExtension(path).ToLower());

                    if (isImage) 
                        return new Bitmap(path);
                    else 
                        return null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading image from path: {path}. Exception: {ex.Message}");
                    return null; // или вернуть изображение-заглушку, если требуется
                }
            }
            return null;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
