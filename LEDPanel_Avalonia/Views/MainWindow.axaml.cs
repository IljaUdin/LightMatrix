using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Media;
using FluentAvalonia.UI.Windowing;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace LEDPanel_Avalonia.Views;

public partial class MainWindow : AppWindow
{
    public MainWindow()
    {
        SplashScreen = new MainAppSplashScreen(this);
        InitializeComponent();
        TitleBar.ExtendsContentIntoTitleBar = true;
        TitleBar.TitleBarHitTestType = TitleBarHitTestType.Complex;
    }

    internal class MainAppSplashScreen : IApplicationSplashScreen
    {
        public MainAppSplashScreen(MainWindow owner)
        {
            _owner = owner;
        }

        public string AppName { get; }
        public IImage AppIcon { get; }
        public object SplashScreenContent => new MainAppSplashContent();
        public int MinimumShowTime => 2000;

        public Action InitApp { get; set; }

        public Task RunTasks(CancellationToken cancellationToken)
        {
            if (InitApp == null)
                return Task.CompletedTask;

            return Task.Run(InitApp, cancellationToken);
        }

        private MainWindow _owner;
    }
}
