using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LEDPanel_Avalonia.Converter
{
    public class MainPicturePathToHeightConverter : IMultiValueConverter
    {
        public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            // values[0] is the MainPicturePath
            // values[1] is the MainPictureHeight
            if (values[0] is string mainPicturePath && !string.IsNullOrEmpty(mainPicturePath) &&
                values[1] is int mainPictureHeight)
            {
                return mainPictureHeight;
            }

            return 0;
        }
    }
}
