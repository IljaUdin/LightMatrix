using LEDPanel_Avalonia.ViewModels;
using Newtonsoft.Json;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace LEDPanel_Avalonia.Model
{
    public class TableConfiguration : ViewModelBase
    {
        MainModel mainModel;
        //private readonly string _filePath = "tableConfiguration.json";
        private readonly string _filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LightMatrix", "tableConfiguration.json");

        #region Размер окна

        [JsonProperty]
        private int _windowWidth;
        public int WindowWidth
        {
            get => _windowWidth;
            set => this.RaiseAndSetIfChanged(ref _windowWidth, value);
        }

        [JsonProperty]
        private double _windowHeight;
        public double WindowHeight
        {
            get => _windowHeight;
            set => this.RaiseAndSetIfChanged(ref _windowHeight, value);
        }

        #endregion

        #region Таймер обновления

        [JsonProperty]
        private int _mediaTimer;
        public int MediaTimer
        {
            get => _mediaTimer;
            set => this.RaiseAndSetIfChanged(ref _mediaTimer, value);
        }

        [JsonProperty]
        private int _textTimer;
        public int TextTimer
        {
            get => _textTimer;
            set => this.RaiseAndSetIfChanged(ref _textTimer, value);
        }

        [JsonProperty]
        private bool _isPhotoAndVideoSelected;
        public bool IsPhotoAndVideoSelected
        {
            get => _isPhotoAndVideoSelected;
            set
            {
                this.RaiseAndSetIfChanged(ref _isPhotoAndVideoSelected, value);
                if (value) IsPhotoSelected = false;
            }
        }

        [JsonProperty]
        private bool _isPhotoSelected;
        public bool IsPhotoSelected
        {
            get => _isPhotoSelected;
            set
            {
                this.RaiseAndSetIfChanged(ref _isPhotoSelected, value);
                if (value) IsPhotoAndVideoSelected = false;
            }
        }

        #endregion

        #region Титульное изображение

        [JsonProperty]
        private int _mainPictureHeight;
        public int MainPictureHeight
        {
            get => _mainPictureHeight;
            set => this.RaiseAndSetIfChanged(ref _mainPictureHeight, value);
        }

        [JsonProperty]
        private int _mainPictureWidth;
        public int MainPictureWidth
        {
            get => _mainPictureWidth;
            set => this.RaiseAndSetIfChanged(ref _mainPictureWidth, value);
        }

        [JsonProperty]
        private int[] _locationMainPicture;
        public int[] LocationMainPicture
        {
            get => _locationMainPicture;
            set => this.RaiseAndSetIfChanged(ref _locationMainPicture, value);
        }

        [JsonProperty]
        private int _locationMainPictureOX;
        public int LocationMainPictureOX
        {
            get => _locationMainPictureOX;
            set
            {
                this.RaiseAndSetIfChanged(ref _locationMainPictureOX, value);
                UpdateLocationMainPicture();
            }
        }

        [JsonProperty]
        private int _locationMainPictureOY;
        public int LocationMainPictureOY
        {
            get => _locationMainPictureOY;
            set
            {
                this.RaiseAndSetIfChanged(ref _locationMainPictureOY, value);
                UpdateLocationMainPicture();
            }
        }

        private void UpdateLocationMainPicture()
        {
            var left = LocationMainPictureOX > 0 ? LocationMainPictureOX : 0;
            var right = LocationMainPictureOX < 0 ? Math.Abs(LocationMainPictureOX) : 0;

            var top = LocationMainPictureOY > 0 ? LocationMainPictureOY : 0;
            var bottom = LocationMainPictureOY < 0 ? Math.Abs(LocationMainPictureOY) : 0;

            LocationMainPicture = new int[] { left, top, right, bottom };
        }

        #endregion

        #region Количество строк

        [JsonProperty]
        private int _rowCount;
        public int RowCount
        {
            get => _rowCount;
            set
            {
                if (value > mainModel?.FuelItems?.Count)
                {
                    mainModel.FuelItems.Add(new FuelItem("", "Type", "10.00"));
                }
                else if (value < mainModel?.FuelItems?.Count)
                {
                    mainModel?.FuelItems?.RemoveAt(value);
                }

                this.RaiseAndSetIfChanged(ref _rowCount, value);
            }
        }

        #endregion

        #region Величина ячеек

        [JsonProperty]
        private int _column1Width;
        public int Column1Width
        {
            get => _column1Width;
            set => this.RaiseAndSetIfChanged(ref _column1Width, value);
        }

        [JsonProperty]
        private int _column2Width;
        public int Column2Width
        {
            get => _column2Width;
            set => this.RaiseAndSetIfChanged(ref _column2Width, value);
        }

        [JsonProperty]
        private int _column3Width;
        public int Column3Width
        {
            get => _column3Width;
            set => this.RaiseAndSetIfChanged(ref _column3Width, value);
        }

        [JsonProperty]
        private int _rowHeight = 30;
        public int RowHeight
        {
            get => _rowHeight;
            set
            {
                foreach (var row in mainModel?.FuelItems)
                {
                    row.RowHeight = value;
                }

                this.RaiseAndSetIfChanged(ref _rowHeight, value);
            }
        }

        #endregion

        #region Расположение строки в ячейке

        [JsonProperty]
        private int _locationCell1OX;
        public int LocationCell1OX
        {
            get => _locationCell1OX;
            set
            {
                this.RaiseAndSetIfChanged(ref _locationCell1OX, value);
                UpdateLocationCell();
            }
        }

        [JsonProperty]
        private int _locationCell2OX;
        public int LocationCell2OX
        {
            get => _locationCell2OX;
            set
            {
                this.RaiseAndSetIfChanged(ref _locationCell2OX, value);
                UpdateLocationCell();
            }
        }

        [JsonProperty]
        private int _locationCell3OX;
        public int LocationCell3OX
        {
            get => _locationCell3OX;
            set
            {
                this.RaiseAndSetIfChanged(ref _locationCell3OX, value);
                UpdateLocationCell();
            }
        }

        [JsonProperty]
        private int _locationCellOY;
        public int LocationCellOY
        {
            get => _locationCellOY;
            set
            {
                this.RaiseAndSetIfChanged(ref _locationCellOY, value);
                UpdateLocationCell();
            }
        }

        private void UpdateLocationCell()
        {
            var left1 = LocationCell1OX > 0 ? LocationCell1OX : 0;
            var right1 = LocationCell1OX < 0 ? Math.Abs(LocationCell1OX) : 0;

            var left2 = LocationCell2OX > 0 ? LocationCell2OX : 0;
            var right2 = LocationCell2OX < 0 ? Math.Abs(LocationCell2OX) : 0;

            var left3 = LocationCell3OX > 0 ? LocationCell3OX : 0;
            var right3 = LocationCell3OX < 0 ? Math.Abs(LocationCell3OX) : 0;

            var top = LocationCellOY > 0 ? LocationCellOY : 0;
            var bottom = LocationCellOY < 0 ? Math.Abs(LocationCellOY) : 0;

            foreach (var row in mainModel.FuelItems)
            {
                row.LocationCell1 = [left1, top, right1, bottom];
                row.LocationCell2 = [left2, top, right2, bottom];
                row.LocationCell3 = [left3, top, right3, bottom];
            }
        }

        #endregion

        #region Размер изображений

        [JsonProperty]
        private int _imageWidth;
        public int ImageWidth
        {
            get => _imageWidth;
            set
            {
                foreach (var row in mainModel?.FuelItems)
                {
                    row.ImageWidth = value;
                }

                this.RaiseAndSetIfChanged(ref _imageWidth, value);
            }
        }

        [JsonProperty]
        private int _imageHeight;
        public int ImageHeight
        {
            get => _imageHeight;
            set
            {
                foreach (var row in mainModel?.FuelItems)
                {
                    row.ImageHeight = value;
                }

                this.RaiseAndSetIfChanged(ref _imageHeight, value);
            }
        }

        #endregion

        #region Работа с текстом

        [JsonProperty]
        private int _textColor;
        public int TextColor
        {
            get => _textColor;
            set => this.RaiseAndSetIfChanged(ref _textColor, value);
        }

        [JsonProperty]
        private double _fontSize;
        public double FontSize
        {
            get => _fontSize;
            set => this.RaiseAndSetIfChanged(ref _fontSize, value);
        }

        [JsonProperty]
        private string _font;
        public string Font
        {
            get => _font;
            set => this.RaiseAndSetIfChanged(ref _font, value);
        }

        #endregion

        #region Отобразить сетку

        private bool _showGridLines;
        public bool ShowGridLines
        {
            get => _showGridLines;
            set => this.RaiseAndSetIfChanged(ref _showGridLines, value);
        }

        #endregion

        public TableConfiguration(MainModel mainModel)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
            this.mainModel = mainModel;
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
                File.AppendAllText("log.txt", $"{DateTime.Now}: Error saving table model - {ex}\n");
            }
        }

        public void Load()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    var json = File.ReadAllText(_filePath);
                    var loadedModel = JsonConvert.DeserializeObject<TableConfiguration>(json);

                    if (loadedModel != null)
                    {
                        this.WindowWidth = loadedModel.WindowWidth;
                        this.WindowHeight = loadedModel.WindowHeight;
                        this.MediaTimer = loadedModel.MediaTimer;
                        this.TextTimer = loadedModel.TextTimer;
                        this.IsPhotoAndVideoSelected = loadedModel.IsPhotoAndVideoSelected;
                        this.IsPhotoSelected = loadedModel.IsPhotoSelected;
                        this.MainPictureHeight = loadedModel.MainPictureHeight;
                        this.MainPictureWidth = loadedModel.MainPictureWidth;
                        this.LocationMainPicture = loadedModel.LocationMainPicture;
                        this.LocationMainPictureOX = loadedModel.LocationMainPictureOX;
                        this.LocationMainPictureOY = loadedModel.LocationMainPictureOY;
                        this.RowCount = loadedModel.RowCount;
                        this.Column1Width = loadedModel.Column1Width;
                        this.Column2Width = loadedModel.Column2Width;
                        this.Column3Width = loadedModel.Column3Width;
                        this.RowHeight = loadedModel.RowHeight;
                        this.LocationCell1OX = loadedModel.LocationCell1OX;
                        this.LocationCell2OX = loadedModel.LocationCell2OX;
                        this.LocationCell3OX = loadedModel.LocationCell3OX;
                        this.LocationCellOY = loadedModel.LocationCellOY;
                        this.ImageWidth = loadedModel.ImageWidth;
                        this.ImageHeight = loadedModel.ImageHeight;
                        this.TextColor = loadedModel.TextColor;
                        this.FontSize = loadedModel.FontSize;
                        this.Font = loadedModel.Font;
                    }
                }
                else
                {
                    WindowWidth = 300;
                    WindowHeight = 350;
                    MediaTimer = 5;
                    TextTimer = 5;
                    IsPhotoAndVideoSelected = true;
                    IsPhotoSelected = false;
                    MainPictureHeight = 50;
                    MainPictureWidth = 50;
                    LocationMainPicture = new int[4];
                    LocationMainPictureOX = 0;
                    LocationMainPictureOY = 0;
                    RowCount = 3;
                    Column1Width = 80;
                    Column2Width = 80;
                    Column3Width = 80;
                    RowHeight = 30;
                    LocationCell1OX = 0;
                    LocationCell2OX = 0;
                    LocationCell3OX = 0;
                    LocationCellOY = 0;
                    ImageWidth = 30;
                    ImageHeight = 30;
                    TextColor = -1;
                    FontSize = 15;
                }
            }
            catch (Exception ex)
            {
                // Логируем ошибку загрузки
                File.AppendAllText("log.txt", $"{DateTime.Now}: Error loading table model - {ex}\n");

                WindowWidth = 300;
                WindowHeight = 350;
                MediaTimer = 5;
                TextTimer = 5;
                IsPhotoAndVideoSelected = true;
                IsPhotoSelected = false;
                MainPictureHeight = 50;
                MainPictureWidth = 50;
                LocationMainPicture = new int[4];
                LocationMainPictureOX = 0;
                LocationMainPictureOY = 0;
                RowCount = 3;
                Column1Width = 80;
                Column2Width = 80;
                Column3Width = 80;
                RowHeight = 30;
                LocationCell1OX = 0;
                LocationCell2OX = 0;
                LocationCell3OX = 0;
                LocationCellOY = 0;
                ImageWidth = 30;
                ImageHeight = 30;
                TextColor = -1;
                FontSize = 15;
            }

        }
    }
}
