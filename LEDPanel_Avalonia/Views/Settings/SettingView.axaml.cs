using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.ReactiveUI;
using LEDPanel_Avalonia.ViewModels.Settings;

namespace LEDPanel_Avalonia.Views.Settings
{
    internal partial class SettingView : ReactiveUserControl<SettingViewModel>
    {
        public SettingView()
        {
            InitializeComponent();
        }

        public void Next(object source, RoutedEventArgs args)
        {
            slides.Next();
        }

        public void Previous(object source, RoutedEventArgs args)
        {
            slides.Previous();
        }
    }
}
