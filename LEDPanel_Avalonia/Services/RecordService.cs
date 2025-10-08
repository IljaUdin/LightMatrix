using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using LEDPanel_Avalonia.Model;
using LEDPanel_Avalonia.ViewModels.Settings;
using LEDPanel_Avalonia.Views.Information;
using Microsoft.Extensions.DependencyInjection;
using Xabe.FFmpeg;

namespace LEDPanel_Avalonia.Services;

public class RecordService : IRecordService
{
    private int GetDurationSeconds()
    {
        int duration = 1;
        
        var settings = App.Current?.Services?.GetService<SettingViewModel>().tableConfiguration;
        
        var mediaFiles = App.Current?.Services?.GetService<MainModel>().MediaFiles;

        if (settings.IsPhotoAndVideoSelected)
        {
            if (mediaFiles.Count == 0)
            {
                duration = settings.TextTimer;
            }
            else
            {
                duration = mediaFiles.Count * settings.MediaTimer + mediaFiles.Count *  settings.TextTimer;
            }
        }
        else
        {
            
        }
        
        
        return duration;
    }
    
    public async Task CreateVideo()
    {
        var window = TopLevel.GetTopLevel(App.Current?.Services?.GetService<InformationWindow>()) as Window;
        var fps = 30;
        var durationSeconds = GetDurationSeconds();
        var totalFrames = fps * durationSeconds;
        var filesService = App.Current?.Services?.GetService<IFilesService>();
        if (filesService is null) throw new NullReferenceException("Missing File Service instance.");

        var path = await filesService.SaveFileDialogAsync();
        
        if (path is null) return;
        
        var tempDir = Path.Combine($"{Path.GetDirectoryName(path)}", $"video_{Guid.NewGuid()}");
        Directory.CreateDirectory(tempDir);

        var frameFiles = new List<string>();

        for (int i = 0; i < totalFrames; i++)
        {
            var pixelSize = new PixelSize((int)window.Bounds.Width, (int)window.Bounds.Height);
            var dpi = new Vector(96, 96);

            using var bitmap = new RenderTargetBitmap(pixelSize, dpi);
            bitmap.Render(window);

            var framePath = Path.Combine(tempDir, $"frame_{i:D5}.png");
            await using var fileStream = File.Create(framePath);
            bitmap.Save(fileStream);
            frameFiles.Add(framePath);

            // Ждём следующий кадр
            await Task.Delay(1000 / fps);
        }

        try
        {
            // Собираем видео
            var conversion = FFmpeg.Conversions.New();
            await conversion.BuildVideoFromImages(frameFiles)
                .SetOutput(path)
                .SetFrameRate(fps)
                .AddParameter("-c:v libx264")
                .AddParameter("-pix_fmt yuv420p")
                .AddParameter("-vf scale=trunc(iw/2)*2:trunc(ih/2)*2") // масштаб до чётных размеров
                .AddParameter("-crf 18")        // качество: 18 = очень высокое, 23 = по умолчанию, 51 = худшее
                .AddParameter("-preset slow")   // медленнее, но лучше качество (ultrafast, superfast, veryfast, faster, fast, medium, slow, slower, veryslow)
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
}