using System.Threading.Tasks;

namespace LightMatrix.Services;

public interface IRecordService
{
    Task CreateVideo(string path);
    Task<int> GetDurationSeconds();
}