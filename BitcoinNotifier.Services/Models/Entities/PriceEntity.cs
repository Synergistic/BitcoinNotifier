using Microsoft.WindowsAzure.Storage.Table;

namespace BitcoinNotifier.Services.Models.Entities
{
    public class PriceEntity : TableEntity
    {
        public PriceEntity() { }
        public string Buy { get; set; }
        public string Sell { get; set; }
        public string Spot { get; set; }
    }
}
