using CtrlMoney.Domain.Entities;
using System.Collections.Generic;

namespace CtrlMoney.Domain.Interfaces.Application
{
    public interface IMovimentService : IApplicationServiceBase<Moviment>
    {
        IEnumerable<Moviment> GetByStartTicketAndYears(string ticketCode, int year);
    }
}
