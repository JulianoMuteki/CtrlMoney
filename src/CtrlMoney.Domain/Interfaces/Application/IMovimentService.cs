using CtrlMoney.Domain.Entities;
using System;
using System.Collections.Generic;

namespace CtrlMoney.Domain.Interfaces.Application
{
    public interface IMovimentService : IApplicationServiceBase<Moviment>
    {
        void DeleteByRangeDate(DateTime dtStart, DateTime dtEnd);
        IEnumerable<Moviment> GetByStartTicketAndYears(string ticketCode, int year);
    }
}
