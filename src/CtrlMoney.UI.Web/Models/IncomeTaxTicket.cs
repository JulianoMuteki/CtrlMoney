using System.Collections.Generic;

namespace CtrlMoney.UI.Web.Models
{
    public class IncomeTaxTicket
    {
        public string TicketCode { get; set; }
        public int Quantity { get; set; }
        public string TotalValue { get; set; }
        public string Bookkeeping { get; set; }
        public ICollection<ResumeBrokerageHistories> ResumeBrokerageHistories { get; set; }
        public ICollection<BrokerageHistoryVM> BrokerageHistoryVMs { get; set; }
    }
}
