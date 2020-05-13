using BitcoinNotifier.Models.Types;
using BitcoinNotifier.Services.Interface;
using BitcoinNotifier.Services.Models.Entities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitcoinNotifier.Services.Implementation
{
    public class StorageService :IStorageService
    {
        private CloudTable AuthTable(string tableName)
        {
            string accountName = "btcnotistorage";
            string accountKey = "7cSbmcyVEcdYPqaw5kGc24AGQxiNNqDxwE8cgacA89GDoHYJ2p0K1Ql9/EqzRoXLLiile4ajngF+tXX6LIrFAw==";
            try
            {
                StorageCredentials creds = new StorageCredentials(accountName, accountKey);
                CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);

                CloudTableClient client = account.CreateCloudTableClient();

                CloudTable table = client.GetTableReference(tableName);

                return table;
            }
            catch
            {
                return null;
            }
        }

        public async Task AddOrUpdate(PriceEntity newEntity)
        {
            var table = AuthTable("Prices");
            TableOperation operation = TableOperation.InsertOrMerge(newEntity);
            await table.ExecuteAsync(operation);
        }

        public async Task AddOrUpdate(AlertEntity newEntity)
        {
            var table = AuthTable("Alerts");
            TableOperation operation = TableOperation.InsertOrMerge(newEntity);
            await table.ExecuteAsync(operation);
        }

        public async Task<PriceEntity> GetPrice(string name)
        {
            var table = AuthTable("Prices");
            TableOperation entity = TableOperation.Retrieve<PriceEntity>("Price", name);
            var result = await table.ExecuteAsync(entity);
            if (result.Result != null)
            {
                return ((PriceEntity)result.Result);
            }
            return null;
        }

        public async Task<List<AlertEntity>> GetAlerts(AlertType alertType = AlertType.None)
        {
            var table = AuthTable("Alerts");
            var entities = await table.ExecuteQuerySegmentedAsync(new TableQuery<AlertEntity>(), null);
            if(alertType != AlertType.None)
            {
                return entities.Where(e => e.AlertType == (int)alertType).ToList();
            }
            return entities.ToList();
        }

        public async Task<AlertEntity> GetAlertById(int id)
        {
            var table = AuthTable("Alerts");
            TableOperation entity = TableOperation.Retrieve<AlertEntity>("Alerts", id.ToString());
            var result = await table.ExecuteAsync(entity);
            if (result.Result != null)
            {
                return ((AlertEntity)result.Result);
            }
            return null;
        }

    }
}
