using LEDPanel_Avalonia.Model;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System.Linq;
using System.Timers;

namespace LEDPanel_Avalonia.ViewModels.Information
{
    internal class InformationViewModel : ViewModelBase
    {
        private int _counter;

        private readonly Timer _timer, _timerDefender;
        public Timer Timer => _timer;

        public MainModel mainModel { get; }
        public TableConfiguration tableConfiguration { get; }

        private ViewModelBase? _currentViewModel;
        public ViewModelBase? CurrentViewModel
        {
            get => _currentViewModel;
            set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
        }

        public InformationViewModel(MainModel mainModel, TableConfiguration tableConfiguration)
        {
            this.mainModel = mainModel;
            this.tableConfiguration = tableConfiguration;

            CurrentViewModel = App.Current?.Services?.GetService<FuelViewModel>();

            _timer = new Timer();
            _timer.Elapsed += OnTimedEvent;
            _timer.Interval = tableConfiguration.TextTimer * 1000; // Start with fuelViewModel display time
            _timer.Start();

            _timerDefender = new Timer();
            _timerDefender.Elapsed += OnTimedEventDefender;
            _timerDefender.Interval = 3600 * 1000;
            _timerDefender.Start();
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            TriggerOnTimedEvent();
        }

        private void OnTimedEventDefender(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            CurrentViewModel = App.Current?.Services?.GetService<DefenderViewModel>();
        }

        public void TriggerOnTimedEvent()
        {
            if (CurrentViewModel == App.Current?.Services?.GetService<FuelViewModel>() && tableConfiguration.MediaTimer > 0 && mainModel?.MediaFiles?.Count > 0)
            {
                CurrentViewModel = App.Current?.Services?.GetService<MediaViewModel>();
                App.Current.Services.GetService<MediaViewModel>().MediaFilePath = mainModel?.MediaFiles?.Skip(_counter).FirstOrDefault();
                _timer.Interval = tableConfiguration.MediaTimer * 1000;
                _counter++;
            }
            else if (CurrentViewModel == App.Current?.Services?.GetService<MediaViewModel>() && tableConfiguration.TextTimer > 0)
            {
                App.Current?.Services?.GetService<MediaViewModel>()?.ReleaseCurrentMedia();
                CurrentViewModel = App.Current?.Services?.GetService<FuelViewModel>();
                _timer.Interval = tableConfiguration.TextTimer * 1000;
            }


            if (_counter >= mainModel?.MediaFiles?.Count)
                _counter = 0;
        }

        public void ResetTimer()
        {
            _timer.Stop();
            _timer.Start();
        }
    }
}
