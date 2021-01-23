using CtrlMoney.Domain.Entities;
using CtrlMoney.Domain.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace CtrlMoney.Domain.Interfaces.Repository
{
    public interface ITransactionRepository : IGenericRepository<FinancialTransaction>
    {
    }
}
