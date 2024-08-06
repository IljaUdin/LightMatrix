using Avalonia;
using Avalonia.Media.Imaging;

namespace LEDPanel_Avalonia.Model
{
    public interface IFuelItem
    {
        string? ProfilePicturePath { get; set; }
        string? FuelType { get; set; }
        string? Price { get; set; }

        int RowHeight { get; set; }

        int ImageWidth { get; set; }
        int ImageHeight { get; set; }

        int[] LocationCell1 { get; set; }
        int[] LocationCell2 { get; set; }
        int[] LocationCell3 { get; set; }
    }
}
