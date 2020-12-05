using CtrlMoney.Domain.Entities.FinancialClassification;
using System;
using System.Collections.Generic;
using System.Text;

namespace CtrlMoney.Domain.Interfaces.Application
{
    public interface IChildTreeAppService: IApplicationServiceBase<ChildTree>
    {
        public ICollection<ChildTree> GetListByParentID(Guid parentID);
    }
}
