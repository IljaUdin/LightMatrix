using LEDPanel_Avalonia.Model;
using LEDPanel_Avalonia.Services;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using FluentAvalonia.UI.Controls;

namespace LEDPanel_Avalonia.ViewModels
{
    internal class DataControlViewModel : ViewModelBase
    {
        private readonly IRecordService _recordService;
        
        public MainModel mainModel { get; }
        public TableConfiguration tableConfiguration {  get; }

        private string? _selectedMediaFile;
        public string? SelectedMediaFile
        {
            get => _selectedMediaFile;
            set => this.RaiseAndSetIfChanged(ref _selectedMediaFile, value);
        }
        
        public ICommand CreateVideoCommand { get; private set; }

        public DataControlViewModel(MainModel mainModel, TableConfiguration tableConfiguration, IRecordService recordService)
        {
            this.mainModel = mainModel;
            this.tableConfiguration = tableConfiguration;
            _recordService =  recordService;

            CreateVideoCommand = ReactiveCommand.CreateFromTask(CreateVideo);
        }

        private async Task CreateVideo()
        {
            try
            {
                await _recordService.CreateVideo();
            }
            catch (Exception ex)
            {
                File.AppendAllText("log.txt", $"{DateTime.Now}: Error create video - {ex}\n");

                var contentDialog = new ContentDialog()
                {
                    Title = "Ошибка",
                    Content = "Для корректной работы приложения требуется установка пакета FFmpeg. \n" +
                              "Пожалуйста, убедитесь, что он установлен и доступен в системе.",
                    PrimaryButtonText = "Ok",
                    DefaultButton = ContentDialogButton.Primary
                };

                await contentDialog.ShowAsync();
            }
        }

        public void SaveModel()
        {
            App.Current?.Services?.GetRequiredService<MainModel>().Save();
        }

        internal async void AddMainPicture()
        {
            try
            {
                var filesService = App.Current?.Services?.GetService<IFilesService>();
                if (filesService is null) throw new NullReferenceException("Missing File Service instance.");

                var file = await filesService.OpenFileImageAsync();
                if (file is null) return;

                mainModel.MainPicturePath = file.Path.LocalPath;
            }
            catch (Exception) { }
        }

        internal  void DeleteMainPicture()
        {
            mainModel.MainPicturePath = null;
        }

        internal async void AddMediaFile()
        {
            try
            {
                var filesService = App.Current?.Services?.GetService<IFilesService>();
                if (filesService is null) throw new NullReferenceException("Missing File Service instance.");

                var file = await filesService.OpenFileMediaAsync();
                if (file is null) return;

                mainModel?.MediaFiles?.Add(file.Path.LocalPath);
            }
            catch (Exception) { }
        }

        internal void DeleteMediaFile()
        {
            mainModel?.MediaFiles?.Remove(SelectedMediaFile);
        }
    }
}