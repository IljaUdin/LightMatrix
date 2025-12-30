using LightMatrix.Model;
using LightMatrix.Services;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using FluentAvalonia.UI.Controls;
using LightMatrix.ViewModels.Information;

namespace LightMatrix.ViewModels
{
    internal class DataControlViewModel : ViewModelBase
    {
        private readonly IRecordService _recordService;
        private readonly InformationViewModel _informationViewModel;


        public MainModel mainModel { get; }
        public TableConfiguration tableConfiguration {  get; }

        private string? _selectedMediaFile;
        public string? SelectedMediaFile
        {
            get => _selectedMediaFile;
            set => this.RaiseAndSetIfChanged(ref _selectedMediaFile, value);
        }
        
        private bool _isCreateVideo;
        public bool IsCreateVideo
        {
            get => _isCreateVideo;
            set => this.RaiseAndSetIfChanged(ref _isCreateVideo, value);
        }
        
        public ICommand CreateVideoCommand { get; private set; }

        public DataControlViewModel(MainModel mainModel, TableConfiguration tableConfiguration, IRecordService recordService, InformationViewModel informationViewModel)
        {
            this.mainModel = mainModel;
            this.tableConfiguration = tableConfiguration;
            _recordService =  recordService;
            _informationViewModel = informationViewModel;

            CreateVideoCommand = ReactiveCommand.CreateFromTask(CreateVideo);
        }

        private async Task CreateVideo()
        {
            try
            {
                IsCreateVideo = true;

                var filesService = App.Current?.Services?.GetService<IFilesService>();
                if (filesService is null) return;

                var path = await filesService.SaveFileDialogAsync();

                if (path == null) return;

                var duration = await _recordService.GetDurationSeconds();

                var contentDialog = new ContentDialog()
                {
                    Title = "Запись экрана",
                    Content = $"Запись займёт примерно  {TimeSpan.FromSeconds(duration):hh\\:mm\\:ss} (часы:минуты:секунды)",
                    PrimaryButtonText = "Ok",
                    DefaultButton = ContentDialogButton.Primary
                };

                contentDialog.ShowAsync();

                _informationViewModel.TriggerOnTimedEvent();

                await _recordService.CreateVideo(path);
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
            finally
            {
                IsCreateVideo = false;
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

                var files = await filesService.OpenFileMediaAsync();
                if (files is null) return;

                foreach (var file in files)
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