using Avalonia.Controls;
using LEDPanel_Avalonia.Model;
using LEDPanel_Avalonia.ViewModels.Settings;
using LEDPanel_Avalonia.Views;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System.Reactive;


namespace LEDPanel_Avalonia.ViewModels;

public class MainViewModel : ViewModelBase, IScreen
{
    private ViewModelBase _dataControlView;
    public ViewModelBase DataControlView
    {
        get => _dataControlView;
        set => this.RaiseAndSetIfChanged(ref _dataControlView, value);
    }

    private IRoutableViewModel _currentViewModel;
    public IRoutableViewModel CurrentViewModel
    {
        get => _currentViewModel;
        set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
    }

    public RoutingState Router { get; }

    public ReactiveCommand<Unit, IRoutableViewModel> GoNext { get; }

    // The command that navigates a user back.
    public ReactiveCommand<Unit, IRoutableViewModel> GoBack => Router.NavigateBack;

    public MainViewModel()
    {
        Router = new RoutingState();

        DataControlView = App.Current?.Services?.GetService<DataControlViewModel>();

        Router.Navigate.Execute(App.Current?.Services?.GetService<PasswordViewModel>());
    }

    public void Load()
    {
        App.Current?.Services?.GetRequiredService<MainModel>().Load();
        App.Current?.Services?.GetRequiredService<TableConfiguration>().Load();
    }

    public void Save()
    {
        App.Current?.Services?.GetRequiredService<MainModel>().Save();
        App.Current?.Services?.GetRequiredService<TableConfiguration>().Save();
    }
}