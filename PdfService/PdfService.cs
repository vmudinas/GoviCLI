using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using static InvoiceService.InvoiceService;
using System.Text;
using System;
using IronPdf;
using System.IO;

namespace PdfService
{
    public partial class PdfService : IPdfService
    {

        private readonly IMemoryCache _cache;
        private readonly ILogger<PdfService> _logger;

        public PdfService(IMemoryCache memoryCache, ILogger<PdfService> logger)
        {
            _cache = memoryCache;
            _logger = logger;
        }

        public void GeneratePdf()
        {

            if (_cache.TryGetValue("Data", out List<Invoice> data))
            {
                // Create a PDF from an existing HTML using C#
                var Renderer = new IronPdf.HtmlToPdf();
                Renderer.PrintOptions.CustomCssUrl = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css");
                Renderer.PrintOptions.FitToPaperWidth = true;
                
                var header = new HtmlHeaderFooter
                {
                    HtmlFragment = GetHTMLHeaderString()
                };
                Renderer.PrintOptions.Header = header;
                var PDF = Renderer.RenderHtmlAsPdf(GetHTMLString(data));
                var OutputPath = $"Invoice{Guid.NewGuid()}.pdf";
                PDF.SaveAs(OutputPath);
                _logger.LogWarning("Report Generated\n");
            }
            else 
            {
                _logger.LogWarning("Cached data not found!");
            }

        }

        public string GetHTMLString(List<Invoice> data)
        {         

            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                  <table align='center'>
                                    <tr>
                                        <th>Id</th>
                                        <th>Amount</th>
                                        <th>Currency</th>
                                        <th>Date</th>
                                        <th>Due</th>
                                        <th>Paid</th>
                                        <th>PaidDate</th>
                                    </tr>");

            foreach (var invoice in data)
            {
                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>    
                                    <td>{3}</td>
                                    <td>{4}</td>    
                                    <td>{5}</td>    
                                    <td>{6}</td>    
                                  </tr>", invoice.Id, invoice.Amount, invoice.Currency, invoice.Date, invoice.Due, invoice.Paid, invoice.PaidDate);
            }

            sb.Append(@"
                                </table>
                            </body>
                        </html>");

            return sb.ToString();
        }
        public string GetHTMLHeaderString()
        {

            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <div class='header'>Invoice Report</div>
                            </body>
                        </html>");

            return sb.ToString();
        }
    }
}
