using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PdfService;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InvoiceService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IInvoiceService _invoiceService;
        private readonly IPdfService _pdfService;

        public Worker(IInvoiceService invoiceService, IPdfService pdfService,  ILogger<Worker> logger)
        {
            _logger = logger;
            _invoiceService = invoiceService;
            _pdfService = pdfService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                Console.WriteLine($"\nPlease select option you want to run:");
                Console.WriteLine($"1.Run goviquery 'invoices' - s 'paidDate' - d 'ASC'");
                Console.WriteLine($"2.Run goviquery 'invoices' - s 'paidDate' - d 'DESC'");
                Console.WriteLine($"3.Run goviquery 'invoices' - s 'amount' -d 'ASC'");
                Console.WriteLine($"4.Run goviquery 'invoices' - s 'amount' - d 'DESC'");
                Console.WriteLine($"5.Display last ran query result from cache");
                Console.WriteLine($"6.Generate Pdf report from cached data\n");
                Console.Write("Enter option number: "); 
       
                await ProcessUserInput(Console.ReadLine());
                
                Console.WriteLine();
            }
        }

        public async Task ProcessUserInput(string userInput)
        {
            try
            {
                switch (userInput)
                {
                    case "1":
                        await _invoiceService.FetchData((x => x.PaidDate), false);
                        break;
                    case "2":
                        await _invoiceService.FetchData((x => x.PaidDate), true);
                        break;
                    case "3":
                        await _invoiceService.FetchData((x => x.Amount), false);
                        break;
                    case "4":
                        await _invoiceService.FetchData((x => x.Amount), true);
                        break;
                    case "5":
                        foreach (var value in _invoiceService.GetCachedData())
                        {
                            Console.WriteLine($"Id: {value.Id} \n Paid: {value.Paid} \n PaidDate: {value.PaidDate} \n Due: {value.Due} \n Date: {value.Date} \n Currency: {value.Currency} \n Amount: {value.Amount}");
                            Console.WriteLine("_______________________________________________");
                        }
                        break;
                    case "6":
                        _pdfService.GeneratePdf();
                        break;
                    default:
                        Console.WriteLine($"Invalid input please try again. \n");
                        break;
                }                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

        }
    }   
}
