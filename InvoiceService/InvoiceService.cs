using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;

namespace InvoiceService
{
    public partial class InvoiceService : IInvoiceService
    {
        private readonly IMemoryCache _cache;
        private readonly HttpClient _httpClient;

        public InvoiceService(IMemoryCache memoryCache, HttpClient httpClient, ILogger<InvoiceService> logger)
        {
            _cache = memoryCache;
            _httpClient = httpClient;
        }

        public async Task FetchData<TColumn>(Func<Invoice,TColumn> sort, bool orderByDesc)
        {           
            //Define Headers
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //Request Token
            Uri loginUri = new Uri("https://bcore-mock.herokuapp.com/invoice");
            var request = await _httpClient.GetAsync(loginUri).ConfigureAwait(false);
            var response = await request.Content.ReadAsStringAsync().ConfigureAwait(false);

            var result = JsonConvert.DeserializeObject<List<Invoice>>(response);

            result = orderByDesc
            ? result.OrderByDescending(sort).ToList()
             : result.OrderBy(sort).ToList();

            //// Save data in cache.
            _cache.Set("Data", result);
        }

        public async Task<List<Invoice>> GetCachedData()
        {
            // Use await here!
            await Task.Delay(10);
            // Save data in cache.
            return  _cache.Get<List<Invoice>>("Data");
        }
    }
}
