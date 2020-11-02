using CtrlMoney.Domain.Entities.FinancialClassification;
using CtrlMoney.Domain.Interfaces.Base;
using System.Collections.Generic;

namespace CtrlMoney.Domain.Interfaces.Repository
{
    public interface IParentTreeRepository : IGenericRepository<ParentTree>
    {
        ICollection<ParentTree> GetAll_WithChildrem();
    }
}
