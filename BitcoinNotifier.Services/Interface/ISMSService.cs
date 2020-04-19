using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinNotifier.Services.Interface
{
    public interface ISMSService
    {
        Task SendMessage(string body, string to);
    }
}
