using System;

namespace CtrlMoney.UI.Web.Models
{
    public class TicketResume
    {
        public string TicketCode { get; set; }
        public string OldTicketCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public string Category { get; set; }
    }
}
