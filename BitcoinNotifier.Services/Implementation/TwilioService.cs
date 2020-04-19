using BitcoinNotifier.Services.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace BitcoinNotifier.Services.Implementation
{
    public class TwilioService :ISMSService
    {
        public async Task SendMessage(string body, string to)
        {
            const string accountSid = "AC7c2ad616125b4851fb3aa78eaa033f7a";
            const string authToken = "1e8c03e60c29143558a981dffaa66945";

            TwilioClient.Init(accountSid, authToken);

            await MessageResource.CreateAsync(
                body: body,
                from: new Twilio.Types.PhoneNumber("+12029492594"),
                to: new Twilio.Types.PhoneNumber(to)
            );

        }
    }
}
