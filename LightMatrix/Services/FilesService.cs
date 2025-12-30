using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightMatrix.Services
{
    internal class FilesService : IFilesService
    {
        private readonly Window _target;

        public FilesService(Window target)
        {
            _target = target;
        }

        public async Task<IStorageFile?> OpenFileImageAsync()
        {
            var files = await _target.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Open File",
                AllowMultiple = false,
                FileTypeFilter = new List<FilePickerFileType>
                {
                    // Define allowed file types here
                    new FilePickerFileType("Image Files")
                    {
                        Patterns = new[] { "*.png", "*.jpg", "*.jpeg", "*.bmp", "*.gif", "*.ico" }
                    }
                }
            });

            return files.Count >= 1 ? files[0] : null;
        }

        public async Task<IReadOnlyList<IStorageFile?>> OpenFileMediaAsync()
        {
            var files = await _target.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Open File",
                AllowMultiple = true,
                FileTypeFilter = new List<FilePickerFileType>
                {
                    // Define allowed file types here
                    new FilePickerFileType("Image Files")
                    {
                        Patterns = new[] { "*.png", "*.jpg", "*.jpeg", "*.bmp", "*.gif", "*.ico" }
                    },
                    new FilePickerFileType("Video Files")
                    {
                        Patterns = new[] { "*.AVI", "*.MP4", "*.MKV", "*.MOV" }
                    }
                }
            });

            return files;
        }

        public async Task<string?> SaveFileDialogAsync()
        {
            var suggestedFileName = "Видеозапись";
            
            var dialog = new SaveFileDialog
            {
                Title = "Сохранить файл",
                InitialFileName = suggestedFileName,
                Filters = new()
                {
                    new() { Name = "Видеофайлы", Extensions = new List<string> { "mp4" } }
                }
            };
            
            var result = await dialog.ShowAsync(_target);

            return result;
        }
    }
}
