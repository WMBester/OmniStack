using WMB.Api.Models;
using System.Threading.Tasks;

namespace WMB.Api.Services
{
    public interface IOpenAIService
    {
        Task<string> GetResponseAsync(string apiKey, string prompt);
    }
}
