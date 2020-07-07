using System.Threading.Tasks;

namespace InvoiceService
{
    public interface IInvoiceService
    {
        Task FetchData();
        Task<string> GetData();
    }
}
