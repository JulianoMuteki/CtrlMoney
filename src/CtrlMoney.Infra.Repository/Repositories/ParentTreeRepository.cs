using CtrlMoney.CrossCutting;
using CtrlMoney.Domain.Entities.FinancialClassification;
using CtrlMoney.Domain.Interfaces.Repository;
using CtrlMoney.Infra.Context;
using CtrlMoney.Infra.Repository.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CtrlMoney.Infra.Repository.Repositories
{
    public class ParentTreeRepository : GenericRepository<ParentTree>, IParentTreeRepository
    {
        public ParentTreeRepository(CtrlMoneyContext context)
            : base(context)
        {

        }

        public ICollection<ParentTree> GetAll_WithChildrem()
        {
            try
            {
                return _context.Set<ParentTree>()
                    .Include(x => x.Children).ThenInclude(x => x.Children)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw CustomException.Create<ParentTree>("Unexpected error fetching GetAll_WithChildrem", nameof(this.GetAll_WithChildrem), ex);
            }
        }
    }
}
