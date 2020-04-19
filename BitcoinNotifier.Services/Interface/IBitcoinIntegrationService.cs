using BitcoinNotifier.Models;
using BitcoinNotifier.Services.Models.Entities;
using System.Threading.Tasks;

namespace BitcoinNotifier.Services.Interface
{
    public interface IBitcoinIntegrationService
    {
        Task<PriceEntity> GetCurrentPriceData();
    }
}
