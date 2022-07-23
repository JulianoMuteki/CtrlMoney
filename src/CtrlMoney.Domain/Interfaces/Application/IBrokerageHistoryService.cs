using CtrlMoney.Domain.Entities;
using System.Collections.Generic;

namespace CtrlMoney.Domain.Interfaces.Application
{
    public interface IBrokerageHistoryService : IApplicationServiceBase<BrokerageHistory>
    {
        ICollection<BrokerageHistory> GetByTicketCode(string ticketCode);
    }
}
