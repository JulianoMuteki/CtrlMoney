using Microsoft.AspNetCore.Identity;
using System;

namespace CtrlMoney.Domain.Identity
{
    public class ApplicationUserLogin : IdentityUserLogin<Guid>
    {
        public virtual ApplicationUser User { get; set; }
    }
}
