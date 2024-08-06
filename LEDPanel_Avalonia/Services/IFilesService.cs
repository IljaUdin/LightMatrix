using Avalonia.Platform.Storage;
using System.Threading.Tasks;

namespace LEDPanel_Avalonia.Services
{
    public interface IFilesService
    {
        public Task<IStorageFile?> OpenFileImageAsync();

        public Task<IStorageFile?> OpenFileMediaAsync();
    }
}