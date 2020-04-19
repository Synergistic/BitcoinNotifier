using BitcoinNotifier.Services.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace BitcoinNotifier.Services.Implementation
{

    public class HttpService: IHttpService
    {
        private HttpClient Client { get; set; }
        private readonly object Locker = new object();
        protected HttpClient CreateClient(string baseUrl)
        {
            var newUri = new Uri($"{baseUrl.TrimEnd('/')}/");
            if (this.Client == null)
            {
                lock(this.Locker)
                {
                    if(this.Client == null)
                    {
                        var output = this.Client = new HttpClient()
                        {
                            BaseAddress = newUri
                        };
                        output.DefaultRequestHeaders.Accept.Clear();
                        output.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        return output;
                    }
                }
            }
            else
            {
                if(newUri != this.Client.BaseAddress)
                {
                    this.Client.Dispose();
                    var output = this.Client = new HttpClient()
                    {
                        BaseAddress = newUri
                    };
                    output.DefaultRequestHeaders.Accept.Clear();
                    output.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                }

            }
            return this.Client;
        }

        public async Task<JObject> Fetch(string fullUrl)
        {
            var fullUri = new Uri(fullUrl);
            using (var request = new HttpRequestMessage(HttpMethod.Get, fullUri.PathAndQuery.TrimStart('/')))
            {
                var response = await this.CreateClient(fullUri.GetLeftPart(UriPartial.Authority)).SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JObject.Parse(json);
                }
                else
                {
                    throw new Exception("Error Fetching: " + fullUrl);
                }

            }

        }
    }
}
