using CtrlMoney.Domain.Entities;
using System;
using System.Collections.Generic;

namespace CtrlMoney.Domain.Interfaces.Application
{
    public interface IEarningService : IApplicationServiceBase<Earning>
    {
        void DeleteByRangeDate(DateTime dtStart, DateTime dtEnd);
        ICollection<Earning> GetByTicketCodeAndBaseYear(string ticketCode, int year);
    }
}
