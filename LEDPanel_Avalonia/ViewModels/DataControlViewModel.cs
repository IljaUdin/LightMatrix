using LEDPanel_Avalonia.Model;
using LEDPanel_Avalonia.Services;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;

namespace LEDPanel_Avalonia.ViewModels
{
    internal class DataControlViewModel : ViewModelBase
    {
        public MainModel mainModel { get; }
        public TableConfiguration tableConfiguration {  get; }

        private string? _selectedMediaFile;
        public string? SelectedMediaFile
        {
            get => _selectedMediaFile;
            set => this.RaiseAndSetIfChanged(ref _selectedMediaFile, value);
        }

        public DataControlViewModel(MainModel mainModel, TableConfiguration tableConfiguration)
        {
            this.mainModel = mainModel;
            this.tableConfiguration = tableConfiguration;
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