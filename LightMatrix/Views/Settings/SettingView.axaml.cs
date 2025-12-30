using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.ReactiveUI;
using LightMatrix.ViewModels.Settings;

namespace LightMatrix.Views.Settings
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
