using CtrlMoney.Domain.Entities;
using System;
using System.Collections.Generic;

namespace CtrlMoney.Domain.Interfaces.Application
{
    public interface IBrokerageHistoryService : IApplicationServiceBase<BrokerageHistory>
    {
        ICollection<BrokerageHistory> GetByTicketCode(string ticketCode, int baseYear);
        ICollection<BrokerageHistory> GetByCategory(string category);
        void DeleteByRangeDate(DateTime dtStart, DateTime dtEnd);
    }
}
