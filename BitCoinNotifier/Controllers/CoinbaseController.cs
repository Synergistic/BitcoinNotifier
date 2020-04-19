using System.Threading.Tasks;
using BitcoinNotifier.Services.Interface;
using BitcoinNotifier.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace BitcoinNotifier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoinbaseController : ControllerBase
    {
        protected readonly ICoinbaseIntegrationService BitcoinIntegrationService;
        protected readonly IStorageService StorageService;
        protected readonly IAlertService AlertService;

        public CoinbaseController(ICoinbaseIntegrationService bitcoinIntegrationService, IStorageService storageService, IAlertService alertService)
        {
            BitcoinIntegrationService = bitcoinIntegrationService;
            StorageService = storageService;
            AlertService = alertService;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            var price = await StorageService.GetPrice("btc");
            return $"Buy: {price.Buy}\nSell: {price.Sell}\nSpot: {price.Spot}";
        }


        [HttpGet("SetAlert")]
        public async Task SetAlert(int alertType, string target, int alertId, string phoneNumber)
        {
            await AlertService.AddAlert((AlertType)alertType, target, alertId, phoneNumber);
        }

        [HttpGet("GetAlerts")]
        public async Task<string> GetAlerts()
        {
            var alerts = await AlertService.GetAlerts();
            var output = string.Empty;
            foreach(var alert in alerts)
            {
                var line = $"ID {alert.RowKey} | Target {alert.Target} | Type {((AlertType)alert.AlertType).ToString()} | Active {alert.Active} | PhoneNumber {alert.PhoneNumber}\n";
                output += line;
            }
            return output;
        }

    }
}