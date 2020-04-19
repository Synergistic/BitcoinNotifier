using BitcoinNotifier.Services.Implementation;
using BitcoinNotifier.Services.Interface;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(PriceChecker.Startup))]

namespace PriceChecker
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<ICoinbaseIntegrationService, CoinbaseIntegrationService>();
            builder.Services.AddSingleton<IStorageService, StorageService>();
            builder.Services.AddSingleton<IHttpService, HttpService>();
            builder.Services.AddSingleton<IAlertService, AlertService>();
            builder.Services.AddSingleton<IHttpService, HttpService>();
            builder.Services.AddSingleton<ISMSService, TwilioService>();
        }
    }
}
