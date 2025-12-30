using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using LightMatrix.Model;
using LightMatrix.Services;
using LightMatrix.ViewModels;
using LightMatrix.ViewModels.Information;
using LightMatrix.ViewModels.Settings;
using LightMatrix.Views;
using LightMatrix.Views.Information;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;

namespace LightMatrix;

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
