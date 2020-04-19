using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitcoinNotifier.Services.Models.Entities
{
    public class AlertEntity :TableEntity
    {
        public int AlertType { get; set; }
        public string Target { get; set; }
        public bool Active { get; set; }
        public string PhoneNumber { get; set; }
    }
}
