using BitcoinNotifier.Models.Types;
using BitcoinNotifier.Services.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BitcoinNotifier.Services.Interface
{
    public interface IStorageService
    {
        Task AddOrUpdate(PriceEntity newEntity);
        Task AddOrUpdate(AlertEntity newEntity);
        Task<PriceEntity> GetPrice(string name);
        Task<List<AlertEntity>> GetAlerts(AlertType alertType = AlertType.None);
        Task<AlertEntity> GetAlertById(int id);
    }
}
