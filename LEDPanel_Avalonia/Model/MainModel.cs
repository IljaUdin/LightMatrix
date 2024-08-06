using Avalonia.Media.Imaging;
using LEDPanel_Avalonia.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace LEDPanel_Avalonia.Model
{
    public class MainModel : ViewModelBase
    {
        //private readonly string _filePath = "mainModel.json";
        private readonly string _filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LightMatrix", "mainModel.json");

        [JsonProperty]
        private ObservableCollection<FuelItem>? _fuelItems;
        public ObservableCollection<FuelItem>? FuelItems
        {
            get => _fuelItems;
            set => this.RaiseAndSetIfChanged(ref _fuelItems, value);
        }

        [JsonProperty]
        private ObservableCollection<string>? _mediaFiles;
        public ObservableCollection<string>? MediaFiles
        {
            get => _mediaFiles;
            set => this.RaiseAndSetIfChanged(ref _mediaFiles, value);
        }

        [JsonProperty]
        private string? _mainPicturePath;
        public string? MainPicturePath
        {
            get => _mainPicturePath;
            set => this.RaiseAndSetIfChanged(ref _mainPicturePath, value);
        }

        public MainModel()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
        }

        public void Save()
        {
            try
            {
                var json = JsonConvert.SerializeObject(this);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                // Логируем ошибку сохранения
                File.AppendAllText("log.txt", $"{DateTime.Now}: Error saving main model - {ex}\n");
            }
        }

        public void Load()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    var json = File.ReadAllText(_filePath);
                    var loadedModel = JsonConvert.DeserializeObject<MainModel>(json);

                    if (loadedModel != null)
                    {
                        this.FuelItems = loadedModel.FuelItems;
                        this.MediaFiles = loadedModel.MediaFiles;
                        this.MainPicturePath = loadedModel.MainPicturePath;
                    }
                }
                else
                {
                    FuelItems = new ObservableCollection<FuelItem>
                    {
                        new FuelItem(null, "АИ-92", "45.50"),
                        new FuelItem(null, "АИ-95", "52.20"),
                        new FuelItem(null, "ДТ", "60.00")
                    };
                    MediaFiles = new ObservableCollection<string>();
                }
            }
            catch (Exception ex)
            {
                // Логируем ошибку загрузки
                File.AppendAllText("log.txt", $"{DateTime.Now}: Error loading main model - {ex}\n");

                FuelItems = new ObservableCollection<FuelItem>
                {
                    new FuelItem(null, "АИ-92", "45.50"),
                    new FuelItem(null, "АИ-95", "52.20"),
                    new FuelItem(null, "ДТ", "60.00")
                };
                MediaFiles = new ObservableCollection<string>();
            }
        }
    }
}
