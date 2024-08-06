using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using LEDPanel_Avalonia.Services;
using LEDPanel_Avalonia.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ReactiveUI;
using System;
using System.IO;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;

namespace LEDPanel_Avalonia.Model
{
    public class FuelItem : ViewModelBase, IFuelItem
    {
        private StreamGeometry? _newImageIcon;
        public StreamGeometry? NewImageIcon
        {
            get => _newImageIcon;
            set => this.RaiseAndSetIfChanged(ref _newImageIcon, value);
        }

        private bool? _isVisibleNewImageIcon;
        public bool? IsVisibleNewImageIcon
        {
            get => _isVisibleNewImageIcon;
            set => this.RaiseAndSetIfChanged(ref _isVisibleNewImageIcon, value);
        }

        [JsonProperty]
        private string? _profilePicturePath;
        public string? ProfilePicturePath
        {
            get => _profilePicturePath;
            set => this.RaiseAndSetIfChanged(ref _profilePicturePath, value);
        }

        [JsonProperty]
        private string? _fuelType;
        public string? FuelType
        {
            get => _fuelType;
            set => this.RaiseAndSetIfChanged(ref _fuelType, value);
        }

        [JsonProperty]
        private string? _price;
        public string? Price
        {
            get => _price;
            set => this.RaiseAndSetIfChanged(ref _price, value);
        }

        #region TableSetting

        [JsonProperty]
        private int _rowHeight = 30;
        public int RowHeight
        {
            get => _rowHeight;
            set => this.RaiseAndSetIfChanged(ref _rowHeight, value);
        }

        #region Размер изображений

        [JsonProperty]
        private int _imageWidth = 30;
        public int ImageWidth
        {
            get => _imageWidth;
            set => this.RaiseAndSetIfChanged(ref _imageWidth, value);
        }

        [JsonProperty]
        private int _imageHeight = 30;
        public int ImageHeight
        {
            get => _imageHeight;
            set => this.RaiseAndSetIfChanged(ref _imageHeight, value);
        }

        #endregion

        #region Расположение строки в ячейке

        [JsonProperty]
        private int[] _locationCell1 = new int[4];
        public int[] LocationCell1
        {
            get => _locationCell1;
            set => this.RaiseAndSetIfChanged(ref _locationCell1, value);
        }

        [JsonProperty]
        private int[] _locationCell2 = new int[4];
        public int[] LocationCell2
        {
            get => _locationCell2;
            set => this.RaiseAndSetIfChanged(ref _locationCell2, value);
        }

        [JsonProperty]
        private int[] _locationCell3 = new int[4];
        public int[] LocationCell3
        {
            get => _locationCell3;
            set => this.RaiseAndSetIfChanged(ref _locationCell3, value);
        }

        #endregion

        #endregion

        public ReactiveCommand<Unit, Unit> DeleteImageCommand { get; }

        public ReactiveCommand<Unit, Unit> AddNewImageCommand { get; }


        //private readonly Action<Person> _deleteAction;

        public FuelItem(string? profilePicturePath, string fuelType, string price) //, Action<Person> deleteAction
        {
            if (File.Exists(profilePicturePath))
            {
                ProfilePicturePath = profilePicturePath;
            }
            else
            {
                Application.Current!.TryFindResource("Add", out var res);
                NewImageIcon = res as StreamGeometry;
            }

            FuelType = fuelType;
            Price = price;
            //_deleteAction = deleteAction;

            DeleteImageCommand = ReactiveCommand.Create(DeleteImage);
            AddNewImageCommand = ReactiveCommand.CreateFromTask(AddNewImage);
        }

        public async Task AddNewImage(CancellationToken token)
        {
            try
            {
                var filesService = App.Current?.Services?.GetService<IFilesService>();
                if (filesService is null) throw new NullReferenceException("Missing File Service instance.");

                var file = await filesService.OpenFileImageAsync();
                if (file is null) return;

                ProfilePicturePath = file.Path.LocalPath;
            }
            catch (Exception) { }
        }

        private void DeleteImage()
        {
            ProfilePicturePath = null;
            Application.Current!.TryFindResource("Add", out var res);
            NewImageIcon = res as StreamGeometry;
        }
    }
}