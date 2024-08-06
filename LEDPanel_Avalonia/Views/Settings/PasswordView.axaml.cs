using Avalonia.Controls;
using Avalonia.ReactiveUI;
using LEDPanel_Avalonia.ViewModels.Settings;

namespace LEDPanel_Avalonia.Views.Settings
{
    internal partial class PasswordView : ReactiveUserControl<PasswordViewModel>
    {
        public PasswordView()
        {
            InitializeComponent();
        }
    }
}