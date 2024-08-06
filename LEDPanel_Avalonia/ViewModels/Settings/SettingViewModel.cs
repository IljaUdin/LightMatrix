using Avalonia.Media;
using LEDPanel_Avalonia.Model;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reactive;

namespace LEDPanel_Avalonia.ViewModels.Settings
{
    public class SettingViewModel : RoutableViewModel
    {
        readonly Func<IScreen> _hostScreenFactory;
        public IScreen HostScreen => _hostScreenFactory();

        public MainModel mainModel { get; }
        public TableConfiguration tableConfiguration { get; }

        public ObservableCollection<string> ListFonts { get; }

        public ReactiveCommand<Unit, Unit> ExitCommand { get; }
        public SettingViewModel(Func<IScreen> hostScreenFactory, MainModel mainModel, TableConfiguration tableConfiguration) : base(hostScreenFactory)
        {
            _hostScreenFactory = hostScreenFactory;
            this.tableConfiguration = tableConfiguration;
            this.mainModel = mainModel;


            ListFonts = new ObservableCollection<string>(FontManager.Current.SystemFonts.Select(f => f.Name).OrderBy(name => name));
            tableConfiguration.Font ??= ListFonts.FirstOrDefault();

            ExitCommand = ReactiveCommand.Create(Exit);
        }

        private void Exit()
        {
            App.Current?.Services?.GetRequiredService<TableConfiguration>().Save();

            HostScreen.Router.Navigate.Execute(App.Current?.Services?.GetService<PasswordViewModel>());
            //(App.Current.Services.GetService<MainViewModel>()).CurrentViewModel = App.Current.Services.GetService<PasswordViewModel>();
        }
    }
}
