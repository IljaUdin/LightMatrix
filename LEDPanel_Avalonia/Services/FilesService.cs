using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LEDPanel_Avalonia.Services
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

        public async Task<IStorageFile?> OpenFileMediaAsync()
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
                    },
                    new FilePickerFileType("Video Files")
                    {
                        Patterns = new[] { "*.AVI", "*.MP4", "*.MKV", "*.MOV" }
                    }
                }
            });

            return files.Count >= 1 ? files[0] : null;
        }
    }
}
