using System;
using Newtonsoft.Json;

namespace InvoiceService
{
    public partial class InvoiceService
    {
        public class Invoice
        {
            [JsonProperty(PropertyName = "id")]
            public Guid Id { get; set; }
            [JsonProperty(PropertyName = "amount")]
            public double Amount { get; set; }
            [JsonProperty(PropertyName = "date")]
            public DateTime Date { get; set; }
            [JsonProperty(PropertyName = "due")]
            public DateTime Due { get; set; }
            [JsonProperty(PropertyName = "paid")]
            public bool Paid { get; set; }
            [JsonProperty(PropertyName = "paidDate")]
            public DateTime PaidDate { get; set; }
            [JsonProperty(PropertyName = "currency")]
            public string Currency { get; set; }        
        }
    }
}
