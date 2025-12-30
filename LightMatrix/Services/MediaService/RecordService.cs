using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using LightMatrix.Model;
using LightMatrix.ViewModels.Information;
using LightMatrix.ViewModels.Settings;
using LightMatrix.Views.Information;
using LibVLCSharp.Shared;
using Microsoft.Extensions.DependencyInjection;
using Xabe.FFmpeg;

namespace LightMatrix.Services;

public class RecordService : IRecordService
{
    private readonly IPlayerService _playerService;

    public async Task<int> GetDurationSeconds()
    {
        int duration = 1;

        var settings = App.Current?.Services?.GetService<SettingViewModel>().tableConfiguration;

        var mediaFiles = App.Current?.Services?.GetService<MainModel>().MediaFiles;

        if (mediaFiles.Count == 0)
        {
            return settings.TextTimer * 2;
        }

        if (settings.IsPhotoAndVideoSelected)
        {
            duration = mediaFiles.Count * settings.TextTimer + mediaFiles.Count * settings.MediaTimer;
        }
        else
        {
            var videoExtensions = new[] { ".avi", ".mp4", ".mkv", ".mov" };

            duration = mediaFiles.Count * settings.TextTimer;

            foreach (var mediaFile in mediaFiles)
            {
                bool isVideo = videoExtensions.Contains(Path.GetExtension(mediaFile).ToLower());

                if (isVideo)
                {
                    using var media = new Media(_playerService.LibVLC, mediaFile, FromType.FromPath);

                    var parseStatus = await media.Parse(
                        options: MediaParseOptions.ParseLocal,
                        timeout: 5000 // 5 секунд таймаут
                    );

                    if (parseStatus == MediaParsedStatus.Done || parseStatus == MediaParsedStatus.Skipped)
                    {
                        duration += (int)(media.Duration / 1000.0);
                    }
                }
                else
                    duration += settings.MediaTimer;
            }
        }

        return duration * 2;
    }

    public async Task CreateVideo(string path)
    {
        var mediaPlayer = _playerService.MediaPlayer;

        var window = TopLevel.GetTopLevel(App.Current?.Services?.GetService<InformationWindow>()) as Window;
        var fps = 30;
        var durationSeconds = await GetDurationSeconds();
        var totalFrames = fps * durationSeconds;

        if (path is null) return;

        var tempDir = Path.Combine($"{Path.GetDirectoryName(path)}", $"video_{Guid.NewGuid()}");
        Directory.CreateDirectory(tempDir);

        var frameFiles = new List<string>();
        var sw = Stopwatch.StartNew();

        for (int i = 0; i < totalFrames; i++)
        {
            var framePath = Path.Combine(tempDir, $"frame_{i:D5}.png");

            if (mediaPlayer.IsPlaying)
            {
                bool isSuccess = await Task.Run(() => mediaPlayer.TakeSnapshot(0, framePath, 0, 0));

                if (!isSuccess || !File.Exists(framePath) || new FileInfo(framePath).Length == 0)
                    await CreateScreenshotWindow(window, framePath);
            }
            else
            {
                await CreateScreenshotWindow(window, framePath);
            }

            frameFiles.Add(framePath);

            // ждём ровно до следующего кадра
            var nextFrameTime = (i + 1) * (1000.0 / fps);
            var delay = nextFrameTime - sw.ElapsedMilliseconds;
            if (delay > 0)
                await Task.Delay((int)delay);
        }

        try
        {
            // Собираем видео
            var conversion = FFmpeg.Conversions.New();
            await conversion.BuildVideoFromImages(frameFiles)
                .SetInputFrameRate(fps)
                .SetOutput(path)
                .SetFrameRate(fps)
                .AddParameter("-c:v libx264")
                .AddParameter("-pix_fmt yuv420p")
                .AddParameter("-vf scale=trunc(iw/2)*2:trunc(ih/2)*2") // масштаб до чётных размеров
                .AddParameter("-crf 18") // качество: 18 = очень высокое, 23 = по умолчанию, 51 = худшее
                .AddParameter("-preset slow") // медленнее, но лучше качество (ultrafast, superfast, veryfast, faster, fast, medium, slow, slower, veryslow)
                .SetOverwriteOutput(true) //перезаписать, если файл с таким именем уже существует
                .UseMultiThread(true)
                .Start();
        }
        finally
        {
            // Очищаем временные файлы
            Directory.Delete(tempDir, true);
        }
    }

    private async Task CreateScreenshotWindow(Window window, string framePath)
    {
        var pixelSize = new PixelSize((int)window.Bounds.Width, (int)window.Bounds.Height);
        var dpi = new Vector(96, 96);

        using var bitmap = new RenderTargetBitmap(pixelSize, dpi);
        bitmap.Render(window);
        await using var fileStream = File.Create(framePath);
        bitmap.Save(fileStream);
    }

    public RecordService(IPlayerService playerService)
    {
        _playerService = playerService;
    }
}