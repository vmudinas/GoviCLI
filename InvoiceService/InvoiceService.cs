using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http;

namespace InvoiceService
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IMemoryCache _cache;
        public InvoiceService(IMemoryCache memoryCache, HttpClient httpClient, ILogger<InvoiceService> logger)
        {
            _cache = memoryCache;
        }
        public async Task FetchData()
        {
            // Save data in cache.
            _cache.Set("Data", DateTime.Now.ToString());

            // Use await here!
            await Task.Delay(10);
            Console.WriteLine("Test");
        }
        public async Task<string> GetData()
        {
            // Use await here!
            await Task.Delay(10);
            // Save data in cache.
            return  _cache.Get<string>("Data");
        }
    }
}
