using Microsoft.AspNetCore.Identity;
using System;

namespace CtrlMoney.Domain.Identity
{
    public class ApplicationUserToken : IdentityUserToken<Guid>
    {
        public virtual ApplicationUser User { get; set; }
    }
}
