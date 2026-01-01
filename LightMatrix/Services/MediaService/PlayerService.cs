using System;
using System.IO;
using System.Linq;
using LightMatrix.Model;
using LightMatrix.ViewModels.Information;
using LibVLCSharp.Shared;

namespace LightMatrix.Services;

public class PlayerService : IPlayerService
{
    #region Fields
    
    private Media? _media;
    private LibVLC _libVLC;
    private MediaPlayer _mediaPlayer;
    
    #endregion
    
    #region Properties
    
    public MediaPlayer MediaPlayer => _mediaPlayer;
    public LibVLC LibVLC => _libVLC;
    
    #endregion
    
    #region Events
    
    public event EventHandler? MediaEnded;
    
    #endregion
    
    #region Methods
    
    private void MediaPlayer_EndReached(object? sender, EventArgs e)
    {
        ReleaseCurrentMedia();
    }

    public void ReleaseCurrentMedia()
    {
        if (_media != null)
        {
            if (_mediaPlayer.IsPlaying)
                _mediaPlayer.Stop();

            _media.Dispose();
            _media = null;
            
            MediaEnded?.Invoke(this, EventArgs.Empty);
        }
    }

    public void UpdateMedia(string mediaFilePath)
    {
        // Play video in the embedded VideoView
        _media = new Media(_libVLC, mediaFilePath, FromType.FromPath);

        _mediaPlayer.Stop();
        _mediaPlayer.Play(_media);
    }
    
    public bool IsVideoFile(string filePath)
    {
        var videoExtensions = new[] { ".avi", ".mp4", ".mkv", ".mov" };
        return !string.IsNullOrWhiteSpace(filePath) && videoExtensions.Contains(Path.GetExtension(filePath).ToLower());
    }
    
    #endregion
    
    public PlayerService()
    {
        Core.Initialize();
        _libVLC = new LibVLC("--no-osd", "--no-sub-autodetect-file");
        
        _mediaPlayer = new MediaPlayer(_libVLC);
        _mediaPlayer.EndReached += MediaPlayer_EndReached;
    }
}