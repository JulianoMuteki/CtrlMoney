using CtrlMoney.Domain.Entities;
using System;
using System.Collections.Generic;

namespace CtrlMoney.Domain.Interfaces.Application
{
    public interface IXLWorkbookService
    {
        IList<BrokerageHistory> ImportTransactionsSheet(string fullfileName);
        IList<Earning> ImportEarningsSheet(string fullfileName);
        IList<Position> ImportPositionsSheet(string fullfileName, DateTime positionDate);
        IList<Moviment> ImportMovimentsSheet(string fullfileName);

        IList<BrokerageHistory> ImportSITransactionsSheet(string fullfileName);
        
    }
}
