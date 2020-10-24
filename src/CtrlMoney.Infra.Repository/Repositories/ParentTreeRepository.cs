using CtrlMoney.Domain.Entities.FinancialClassification;
using CtrlMoney.Domain.Interfaces.Repository;
using CtrlMoney.Infra.Context;
using CtrlMoney.Infra.Repository.Common;

namespace CtrlMoney.Infra.Repository.Repositories
{
    public class ParentTreeRepository : GenericRepository<ParentTree>, IParentTreeRepository
    {
        public ParentTreeRepository(CtrlMoneyContext context)
            : base(context)
        {

        }
    }
}
