using Avalonia.Controls;
using Avalonia.ReactiveUI;
using LightMatrix.ViewModels.Settings;

namespace LightMatrix.Views.Settings
{
    internal partial class PasswordView : ReactiveUserControl<PasswordViewModel>
    {
        public PasswordView()
        {
            InitializeComponent();
        }
    }
}