using System.Threading.Tasks;

namespace WorstUrlShortener.Interfaces
{
    public interface IScreen
    {
        Task<byte[]> CaptureScreenAsync();
    }
}
