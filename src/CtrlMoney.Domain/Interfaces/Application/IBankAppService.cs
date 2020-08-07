using CtrlMoney.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CtrlMoney.Domain.Interfaces.Application
{
    public interface IBankAppService : IApplicationServiceBase<Bank>
    {
        ICollection<Bank> GetAllBanks();
    }
}
