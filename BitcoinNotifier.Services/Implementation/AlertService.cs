using BitcoinNotifier.Models.Types;
using BitcoinNotifier.Services.Interface;
using BitcoinNotifier.Services.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BitcoinNotifier.Services.Implementation
{
    public class AlertService : IAlertService
    {
        protected IStorageService StorageService;
        protected ISMSService TwilioService;
        public AlertService(IStorageService storageService, ISMSService twilioService)
        {
            StorageService = storageService;
            TwilioService = twilioService;
        }
        public async Task AddAlert(AlertType alertType, string target, int alertId, string phoneNumber)
        {
            var alertEntity = new AlertEntity()
            {
                Active = true,
                AlertType = (int)alertType,
                PartitionKey = "Alerts",
                PhoneNumber = $"{phoneNumber}",
                Target = target,
                RowKey = alertId.ToString()

            };
            await StorageService.AddOrUpdate(alertEntity);
        }

        public async Task<List<AlertEntity>> GetAlerts(AlertType alertType = AlertType.None)
        {
            return await StorageService.GetAlerts(alertType);
        }

        public async Task ProcessAlerts(string newPrice, string oldPrice)
        {
            AlertType targetDirection;
            Decimal latestSell = Decimal.Parse(newPrice);
            Decimal oldSell = Decimal.Parse(oldPrice);
            Compare compareDel = GreaterThan;

            if (latestSell > oldSell)
            {
                targetDirection = AlertType.GreaterThan;
                compareDel = GreaterThan;

            }
            else if (latestSell < oldSell)
            {
                targetDirection = AlertType.LessThan;
                compareDel = LessThan;
            }
            else
            {
                targetDirection = AlertType.None;
            }
            if (targetDirection == AlertType.None) return;

            var alerts = await this.GetAlerts(targetDirection);

            foreach (var alert in alerts)
            {
                if (!alert.Active) continue;
                if(compareDel(latestSell, Decimal.Parse(alert.Target)))
                {
                    await SendNotification(alert.PhoneNumber, alert.Target, newPrice);
                    alert.Active = false;
                    await StorageService.AddOrUpdate(alert);
                }
            }
        }

        private delegate bool Compare(Decimal valueA, Decimal valueB);

        private bool GreaterThan(Decimal valueA, Decimal valueB)
        {
            return (valueA) > (valueB);
        }
        private bool LessThan(Decimal valueA, Decimal valueB)
        {
            return (valueA) < (valueB);
        }

        private async Task SendNotification(string phoneNumber, string thresholdPrice, string currentPrice)
        {
            string body = $"BTC has crossed your threshold price of {thresholdPrice}. It is currently {currentPrice} as of {DateTime.Now}.";
            await TwilioService.SendMessage(body, phoneNumber);
        }


    }
}
