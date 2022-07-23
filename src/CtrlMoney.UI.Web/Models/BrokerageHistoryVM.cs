using System;

namespace CtrlMoney.UI.Web.Models
{
    public class BrokerageHistoryVM
    {
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }       
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
