using CtrlMoney.Domain.Entities;
using System;
using System.Collections.Generic;

namespace CtrlMoney.Domain.Interfaces.Application
{
    public interface IXLWorkbookService
    {
        IList<BrokerageHistory> ImportTransactionsB3Sheet(string fullfileName, IDictionary<string, string> keyTickersCategories);
        IList<Earning> ImportEarningsB3Sheet(string fullfileName, IDictionary<string, string> keyTickersCategories);
        IList<Moviment> ImportMovimentsB3Sheet(string fullfileName, IDictionary<string, string> keyTickersCategories);
        IList<Position> ImportPositionsSheet(string fullfileName, DateTime positionDate);

        IList<BrokerageHistory> ImportSITransactionsSheet(string fullfileName);
        IList<Earning> ImportSIEarningsSheet(string fullfileName);

    }
}
