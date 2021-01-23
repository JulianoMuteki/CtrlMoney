using CtrlMoney.Domain.Entities;
using CtrlMoney.Domain.Interfaces.Repository;
using CtrlMoney.Infra.Context;
using CtrlMoney.Infra.Repository.Common;

namespace CtrlMoney.Infra.Repository.Repositories
{
    public class TransactionRepository: GenericRepository<FinancialTransaction>, ITransactionRepository
    {
        public TransactionRepository(CtrlMoneyContext context)
            : base(context)
        {

        }
    }
}
