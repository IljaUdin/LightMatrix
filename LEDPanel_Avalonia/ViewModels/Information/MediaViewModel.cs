using LEDPanel_Avalonia.Model;
using LibVLCSharp.Shared;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LEDPanel_Avalonia.ViewModels.Information
{
    internal class MediaViewModel : ViewModelBase
    {
        public TableConfiguration? tableConfiguration { get; }

        private LibVLC _libVLC;
        private Media? media;
        public Media? Media;

        private MediaPlayer _mediaPlayer;
        public MediaPlayer MediaPlayer => _mediaPlayer;

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

        public MediaViewModel(TableConfiguration tableConfiguration)
        {
            this.tableConfiguration = tableConfiguration;

            Core.Initialize();
            _libVLC = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVLC);
            _mediaPlayer.EndReached += MediaPlayer_EndReached;
        }

        private void MediaPlayer_EndReached(object? sender, EventArgs e)
        {
            ReleaseCurrentMedia();
        }

        public void ReleaseCurrentMedia()
        {
            if (media != null)
            {
                if (_mediaPlayer.IsPlaying)
                    _mediaPlayer.Stop();

                media.Dispose();
                media = null;

                App.Current?.Services?.GetService<InformationViewModel>()?.ResetTimer();
                App.Current?.Services?.GetService<InformationViewModel>()?.TriggerOnTimedEvent();
            }
        }

        private async void UpdateMedia()
        {
            if (IsVideoFile(_mediaFilePath))
            {
                await Task.Delay(500); // Пауза необходима, чтобы видео не запускалось в отдельном окне (ошибка логики плеера)

                if (tableConfiguration?.IsPhotoAndVideoSelected == false)
                    App.Current?.Services?.GetService<InformationViewModel>()?.Timer.Stop();

                // Play video in the embedded VideoView
                media = new Media(_libVLC, _mediaFilePath, FromType.FromPath);

                _mediaPlayer.Stop();
                _mediaPlayer.Play(media);

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

        private bool IsVideoFile(string filePath)
        {
            var videoExtensions = new[] { ".avi", ".mp4", ".mkv", ".mov" };
            return !string.IsNullOrWhiteSpace(filePath) && videoExtensions.Contains(System.IO.Path.GetExtension(filePath).ToLower());
        }
    }
}
