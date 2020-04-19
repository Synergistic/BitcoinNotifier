using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitcoinNotifier.Models
{
    public class CoinbasePriceDTO :TableEntity
    {
        public string name { get; set; }
        public string amount { get; set; }
    }
}
