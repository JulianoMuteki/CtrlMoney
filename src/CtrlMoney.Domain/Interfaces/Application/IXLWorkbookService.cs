using CtrlMoney.Domain.Entities;
using System.Collections.Generic;

namespace CtrlMoney.Domain.Interfaces.Application
{
    public interface IXLWorkbookService
    {
        IList<BrokerageHistory> ImportSheet();
    }
}
