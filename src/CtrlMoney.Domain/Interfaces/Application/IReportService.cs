using CtrlMoney.DataTransfer;
using CtrlMoney.Domain.Entities;

using System.Collections.Generic;

namespace CtrlMoney.Domain.Interfaces.Application
{
    public interface IReportService
    {
        IEnumerable<BrokerageHistory> GetBrokeragesHistoriesByCategory(string category);
        IList<CompositionStocksDto> GetCompositionTotalStocks(string category);
    }
}
