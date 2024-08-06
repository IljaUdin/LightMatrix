using LEDPanel_Avalonia.Model;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.Reactive;

namespace LEDPanel_Avalonia.ViewModels.Settings
{
    public class PasswordViewModel : RoutableViewModel
    {
        readonly Func<IScreen> _hostScreenFactory;
        public IScreen HostScreen => _hostScreenFactory();

        const int passwordSettings = 111;

        private int? _password;
        public int? Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        private string? _invalidPassword;
        public string? InvalidPassword
        {
            get => _invalidPassword;
            set => this.RaiseAndSetIfChanged(ref _invalidPassword, value);
        }
        
        public ReactiveCommand<Unit, Unit> ApplyCommand { get; }

        public PasswordViewModel(Func<IScreen> hostScreenFactory) : base(hostScreenFactory)
        {
            _hostScreenFactory = hostScreenFactory;

            ApplyCommand = ReactiveCommand.Create(Apply);
        }

        //public event Action? Authorized;

        private void Apply()
        {
            if (Password == passwordSettings)
            {
                InvalidPassword = "";
                Password = null;
                HostScreen.Router.Navigate.Execute(App.Current?.Services?.GetService<SettingViewModel>());
                //(App.Current.Services.GetService<MainViewModel>()).CurrentViewModel = App.Current.Services.GetService<SettingViewModel>();
            }
            else
            {
                InvalidPassword = "Неверный пароль";
            }

        }
    }
}
