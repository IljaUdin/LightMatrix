using LightMatrix.Model;
using LibVLCSharp.Shared;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.Linq;
using System.Threading.Tasks;
using LightMatrix.Services;

namespace LightMatrix.ViewModels.Information
{
    internal class MediaViewModel : ViewModelBase
    {
        private readonly InformationViewModel _informationViewModel;
        private readonly IPlayerService _playerService;
        
        public TableConfiguration? tableConfiguration { get; }
        public MediaPlayer MediaPlayer { get; }

        private bool _isImageVisible;
        public bool IsImageVisible
        {
            get => _isImageVisible;
            set => this.RaiseAndSetIfChanged(ref _isImageVisible, value);
        }

        private bool _isVideoVisible;
        public bool IsVideoVisible
        {
            get => _isVideoVisible;
            set => this.RaiseAndSetIfChanged(ref _isVideoVisible, value);
        }

        private string? _mediaFilePath;
        public string? MediaFilePath
        {
            get => _mediaFilePath;
            set
            {
                this.RaiseAndSetIfChanged(ref _mediaFilePath, value);
                UpdateMedia();
            }
        }

        public MediaViewModel(TableConfiguration tableConfiguration, IPlayerService playerService, InformationViewModel informationViewModel)
        {
            this.tableConfiguration = tableConfiguration;
            _informationViewModel = informationViewModel;
            _playerService = playerService;
            
            MediaPlayer = playerService.MediaPlayer;
        }
        
        private async Task UpdateMedia()
        {
            if (_playerService.IsVideoFile(_mediaFilePath))
            {
                await Task.Delay(500); // Пауза необходима, чтобы видео не запускалось в отдельном окне (ошибка логики плеера)

                if (tableConfiguration?.IsPhotoAndVideoSelected == false)
                    _informationViewModel.Timer.Stop();

                _playerService.UpdateMedia(_mediaFilePath);

                IsImageVisible = false;
                IsVideoVisible = true;   
            }
            else
            {
                // Show image
                IsImageVisible = true;
                IsVideoVisible = false;
            }
        }
    }
}
