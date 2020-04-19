using System;
using System.Threading.Tasks;
using BitcoinNotifier.Services.Interface;
using BitcoinNotifier.Services.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace PriceChecker
{
    public class UpdatePrices
    {

        private readonly ICoinbaseIntegrationService CoinbaseIntegrationService;
        private readonly IStorageService StorageService;
        private readonly IAlertService AlertService;


        public UpdatePrices(ICoinbaseIntegrationService coinbaseIntegrationService, IStorageService storageService, IAlertService alertService)
        {
            CoinbaseIntegrationService = coinbaseIntegrationService;
            StorageService = storageService;
            AlertService = alertService;
        }

        [FunctionName("UpdatePrices")]
        public async Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            var latestPriceData = await CoinbaseIntegrationService.GetCurrentPriceData();
            var oldPriceData = await StorageService.GetPrice("btc");

            await StorageService.AddOrUpdate(latestPriceData);
            await AlertService.ProcessAlerts(latestPriceData.Sell, oldPriceData.Sell);
        }
    }
}
