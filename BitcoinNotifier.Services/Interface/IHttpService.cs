using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace BitcoinNotifier.Services.Interface
{
    public interface IHttpService
    {
        Task<JObject> Fetch(string fullUrl);
    }
}
