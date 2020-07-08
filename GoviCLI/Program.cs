using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PdfService;

namespace InvoiceService
{
    public class Program
    {
        public static void Main(string[] args)
        {         

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging => logging.SetMinimumLevel(LogLevel.Warning))
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddTransient<IInvoiceService, InvoiceService>();
                    services.AddTransient<IPdfService, PdfService.PdfService>();
                    services.AddHttpClient<IInvoiceService, InvoiceService>();
                    services.AddMemoryCache();
                });
    }
}
