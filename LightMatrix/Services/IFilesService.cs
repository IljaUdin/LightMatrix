using Avalonia.Platform.Storage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LightMatrix.Services
{
    public interface IFilesService
    {
        public Task<IStorageFile?> OpenFileImageAsync();

        public Task<IReadOnlyList<IStorageFile?>> OpenFileMediaAsync();

        public Task<string> SaveFileDialogAsync();
    }
}