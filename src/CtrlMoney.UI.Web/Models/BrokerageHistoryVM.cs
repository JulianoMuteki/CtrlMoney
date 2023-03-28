using System;

namespace CtrlMoney.UI.Web.Models
{
    public class BrokerageHistoryVM
    {
        public string TicketCode { get; set; }
        public string OldTicketCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }       
        public decimal Quantity { get; set; }
        public string Price { get; set; }
        public string TotalPrice { get; set; }
    }
}
