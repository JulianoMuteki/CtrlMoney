using CtrlMoney.Domain.Entities.FinancialClassification;
using CtrlMoney.Domain.Interfaces.Repository;
using CtrlMoney.Infra.Context;
using CtrlMoney.Infra.Repository.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CtrlMoney.Infra.Repository.Repositories
{
   public class ChildTreeRepository : GenericRepository<ChildTree>, IChildTreeRepository
    {
        public ChildTreeRepository(CtrlMoneyContext context)
            : base(context)
        {

        }
    }
}
