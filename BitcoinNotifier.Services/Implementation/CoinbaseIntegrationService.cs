using BitcoinNotifier.Models;
using BitcoinNotifier.Models.Types;
using BitcoinNotifier.Services.Interface;
using BitcoinNotifier.Services.Models.Entities;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BitcoinNotifier.Services.Implementation
{
    public class CoinbaseIntegrationService : ICoinbaseIntegrationService
    {
        protected IHttpService HttpService;

        public CoinbaseIntegrationService(IHttpService httpService)
        {
            HttpService = httpService;
        }

        private async Task<CoinbasePriceDTO> GetCurrentPrice(string priceType = PriceType.Spot, string currency = Currency.USD)
        {
            var newUri = $"https://api.coinbase.com/v2/prices/{priceType}?currency={currency}";
            var result = await this.HttpService.Fetch(newUri);
            if(result!= null)
            {
                var value = result.First.First.ToString();
                return JsonConvert.DeserializeObject<CoinbasePriceDTO>(value);
            }
            return null;
        }

        public async Task<PriceEntity> GetCurrentPriceData()
        {
            return new PriceEntity()
            {
                Buy = (await this.GetCurrentPrice(PriceType.Buy)).amount,
                Sell = (await this.GetCurrentPrice(PriceType.Sell)).amount,
                Spot = (await this.GetCurrentPrice(PriceType.Spot)).amount,
                RowKey = "btc",
                PartitionKey = "Price"
            };
        }

    }
}
