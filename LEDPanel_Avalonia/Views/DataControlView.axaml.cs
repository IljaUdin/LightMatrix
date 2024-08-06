using Avalonia.Controls;
using LEDPanel_Avalonia.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace LEDPanel_Avalonia.Views;

public partial class DataControlView : UserControl
{
    public DataControlView()
    {
        InitializeComponent();
    }

    private void TextBlock_PointerEntered(object? sender, Avalonia.Input.PointerEventArgs e)
    {
        if (sender is TextBlock textBlock)
        {
            // Получаем элемент, к которому привязан TextBlock
            var mediaFile = textBlock.DataContext as string;
            if (mediaFile != null)
            {
                App.Current.Services.GetService<DataControlViewModel>().SelectedMediaFile = mediaFile;
            }
        }
    }
}
