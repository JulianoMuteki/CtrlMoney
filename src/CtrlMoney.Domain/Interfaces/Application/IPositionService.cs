using CtrlMoney.Domain.Entities;
using System.Collections.Generic;

namespace CtrlMoney.Domain.Interfaces.Application
{
    public interface IPositionService : IApplicationServiceBase<Position>
    {
        Position GetLatestYearByTicketCode(string ticketCode);
    }
}
