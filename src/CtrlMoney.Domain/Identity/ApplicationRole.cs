using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace CtrlMoney.Domain.Identity
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
        public ICollection<ApplicationRoleClaim> RoleClaims { get; set; }


        public ApplicationRole()
        {

        }

        public ApplicationRole(string roleName)
            : base(roleName)
        {

        }
    }
}
