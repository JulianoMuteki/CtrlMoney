using System.Collections.Generic;

namespace CtrlMoney.UI.Web.Models
{
    public class ResumeBrokerageHistories
    {
        public int Year { get; set; }
        public string TicketCode { get; set; }
        public IList<TransactionYear> TransactionsYears { get; set; }
        public IList<EarningReport> EarningsReport { get; set; }

    }

    public class TransactionYear
    {
        public string TransactionType { get; set; }
        public string TotalValue { get; set; }
        public decimal Quantity { get; set; }
        public string UnitPrice { get; set; }
    }

    public class EarningReport
    {
        public string EventType { get; set; }
        public decimal Quantity { get; set; }
        public string TotalValue { get; set; }

    }

    public class DataOperation
    {
        public decimal Quantity { get; set; }
        public string TotalValue { get; set; }
        public string Operation { get; set; }
        public string TotalLastYear { get; set; }
        public string TotalCalendarYear { get; set; }
        public IList<BrokerageHistoryVM> BrokeragesHistories { get; set; }        
    }

}

               