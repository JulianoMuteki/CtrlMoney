using System;

namespace CtrlMoney.UI.Web.Models
{
    public class TicketConversionVM
    {
        public Guid TicketConversionId { get; set; }
        public string TicketInput { get; set; }
        public string TicketOutput { get; set; }
        public string StockBroker { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime Date { get; set; }
    }
}
