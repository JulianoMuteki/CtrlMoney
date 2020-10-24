using CtrlMoney.Domain.Entities.FinancialClassification;
using CtrlMoney.Domain.Interfaces.Repository;
using CtrlMoney.Infra.Context;
using CtrlMoney.Infra.Repository.Common;

namespace CtrlMoney.Infra.Repository.Repositories
{
    public class GrandChildTreeRepository : GenericRepository<GrandChildTree>, IGrandChildTreeRepository
    {
        public GrandChildTreeRepository(CtrlMoneyContext context)
            : base(context)
        {

        }
    }
}
