using CtrlMoney.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CtrlMoney.Domain.Interfaces.Application
{
    public interface ISecurityApplicationService
    {
        IList<ApplicationUser> GetAllUsers();
    }
}
