using System;
using LibVLCSharp.Shared;

namespace LightMatrix.Services;

public interface IPlayerService
{ 
    LibVLC LibVLC { get; }
    MediaPlayer MediaPlayer { get; }
    
    event EventHandler? MediaEnded;

    bool IsVideoFile(string filePath);
    void UpdateMedia(string mediaFilePath);
    void ReleaseCurrentMedia();
}