using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http;
using System.Collections.Generic;
using static InvoiceService.InvoiceService;
using System.Text;
using DinkToPdf;
using System.IO;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;

namespace PdfService
{
    public partial class PdfService : IPdfService
    {

        private IConverter _converter;
        private readonly IMemoryCache _cache;

        public PdfService(IMemoryCache memoryCache, IConverter converter, ILogger<PdfService> logger)
        {
            _converter = converter;
            _cache = memoryCache;
        }

        public void GeneratePdf()
        {

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "PDF Report",
                Out = Path.Combine(Directory.GetCurrentDirectory(), "assets", "Report.pdf")
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = GetHTMLString(),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            try
            {
                _converter.Convert(pdf);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            // }
        }
        public string GetHTMLString()
        {
            var invoices = _cache.Get<List<Invoice>>("Data");

            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <div class='header'><h1>This is the generated PDF report!!!</h1></div>
                                <table align='center'>
                                    <tr>
                                        <th>ID</th>
                                    </tr>");

            foreach (var invoice in invoices)
            {
                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>                                  
                                  </tr>", invoice.Id);
            }

            sb.Append(@"
                                </table>
                            </body>
                        </html>");

            return sb.ToString();
        }
    }
}
