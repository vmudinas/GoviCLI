using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static InvoiceService.InvoiceService;

namespace InvoiceService
{
    public interface IInvoiceService
    {
        Task FetchData<TColumn>(Func<Invoice, TColumn> sort, bool orderByDesc);
        List<Invoice> GetCachedData();
    }
}
