using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
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

namespace LEDPanel_Avalonia;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var startup = new Startup(desktop);

            Services = startup.GetServices();

            startup.Load();

            desktop.MainWindow = new MainWindow
            {
                DataContext = Services.GetRequiredService<MainViewModel>()
            };

            desktop.MainWindow.Closed += (sender, e) =>
            {
                //startup.Save();
                // Завершаем приложение при закрытии главного окна
                desktop.Shutdown();
            };

            var informationWindow = Services.GetRequiredService<InformationWindow>();
            informationWindow.DataContext = Services.GetRequiredService<InformationViewModel>();

            informationWindow.Show();
        }

        base.OnFrameworkInitializationCompleted();
    }

    public static App? Current => Application.Current as App;

    /// <summary>
    /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
    /// </summary>
    public IServiceProvider? Services { get; private set; }
}
