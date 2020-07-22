using CtrlMoney.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace CtrlMoney.Domain.Identity
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        public virtual ICollection<ApplicationUserClaim> UserClaims { get; set; }
        public virtual ICollection<ApplicationUserLogin> UserLogins { get; set; }
        public virtual ICollection<ApplicationUserToken> UserTokens { get; set; }
        
    }
}
