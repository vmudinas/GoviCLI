using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InvoiceService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IInvoiceService _invoiceService;

        public Worker(IInvoiceService invoiceService, ILogger<Worker> logger)
        {
            _logger = logger;
            _invoiceService = invoiceService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Information - Worker running at: {time}", DateTimeOffset.Now);
                Console.WriteLine($"Please pass invoice query");
                Console.ReadLine();
                await _invoiceService.FetchData();
                var value = await _invoiceService.GetData();

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
    public class Worker2 : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IInvoiceService _invoiceService;

        public Worker2(IInvoiceService invoiceService, ILogger<Worker> logger)
        {
            _logger = logger;
            _invoiceService = invoiceService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("XInformation - Worker running at: {time}", DateTimeOffset.Now);
                Console.WriteLine($"XPlease pass invoice query");
                Console.ReadLine();
                await _invoiceService.FetchData();
                var value = await _invoiceService.GetData();

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
