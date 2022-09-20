using CtrlMoney.Domain.Entities;
using System.Collections.Generic;

namespace CtrlMoney.Domain.Interfaces.Application
{
    public interface IEarningService : IApplicationServiceBase<Earning>
    {
        ICollection<Earning> GetByTicketCodeAndBaseYear(string ticketCode, int year);
    }
}
