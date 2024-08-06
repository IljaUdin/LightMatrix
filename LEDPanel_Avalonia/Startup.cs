using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using LEDPanel_Avalonia.Model;
using LEDPanel_Avalonia.Services;
using LEDPanel_Avalonia.ViewModels;
using LEDPanel_Avalonia.ViewModels.Information;
using LEDPanel_Avalonia.ViewModels.Settings;
using LEDPanel_Avalonia.Views;
using LEDPanel_Avalonia.Views.Information;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;


namespace LEDPanel_Avalonia
{
    internal class Startup
    {
        IClassicDesktopStyleApplicationLifetime desktop;
        private IServiceProvider Services { get; set; }

        public Startup(IClassicDesktopStyleApplicationLifetime desktop)
        {
            var services = new ServiceCollection();

            services.AddSingleton<MainModel>();
            services.AddSingleton<TableConfiguration>();
            services.AddSingleton<IFilesService>(x => new FilesService(desktop.MainWindow));
            services.AddSingleton<IScreen>(provider => provider.GetRequiredService<MainViewModel>());
            services.AddSingleton<Func<IScreen>>(provider => () => provider.GetRequiredService<IScreen>());
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<PasswordViewModel>();
            services.AddSingleton<SettingViewModel>();
            services.AddSingleton<InformationWindow>();
            services.AddSingleton<InformationViewModel>();
            services.AddSingleton<FuelViewModel>();
            services.AddSingleton<MediaViewModel>();
            services.AddSingleton<DataControlView>();
            services.AddSingleton<DataControlViewModel>();

            services.AddSingleton<DefenderViewModel>();

            Services = services.BuildServiceProvider();

            this.desktop = desktop;
            CloseProgram();
        }

        public IServiceProvider GetServices()
        {
            if (Services == null)
            {
                throw new InvalidOperationException("Services have not been initialized.");
            }

            return Services;
        }

        public void Load()
        {
            Services.GetRequiredService<MainModel>().Load();
            Services.GetRequiredService<TableConfiguration>().Load();
        }

        public void Save()
        {
            Services.GetRequiredService<MainModel>().Save();
            Services.GetRequiredService<TableConfiguration>().Save();
        }

        private void CloseProgram()
        {
            DispatcherTimer timerKillProgram = new DispatcherTimer();
            timerKillProgram.Interval = TimeSpan.FromHours(2);
            timerKillProgram.Tick += Timer_CloseProgram;
            timerKillProgram.Start();
        }

        private void Timer_CloseProgram(object sender, EventArgs e)
        {
            desktop.Shutdown();
        }
    }
}
