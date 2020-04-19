using BitcoinNotifier.Models.Types;
using BitcoinNotifier.Services.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BitcoinNotifier.Services.Interface
{
    public interface IAlertService
    {
        Task<List<AlertEntity>> GetAlerts(AlertType alertType = AlertType.None);
        Task AddAlert(AlertType alertType, string target, int alertId, string phoneNumber);
        Task ProcessAlerts(string newPrice, string oldPrice);
    }
}
