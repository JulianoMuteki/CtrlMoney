using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace CtrlMoney.Domain.Security
{
    public static class PolicyTypes
    {
        public const string NAME = "default.policy";

        public static class DeliveryPolicy
        {
            public const string ExecuteDelivery = "delivery.policy.execute";
        }

        public static IList<string> ListAllRoles
        {
            get
            {
                var roles = new List<string>();
                foreach (var role in Enum.GetNames(typeof(RoleAuthorize)))
                {
                    var result = (RoleAuthorize)Enum.Parse(typeof(RoleAuthorize), role);
                    roles.Add(role);
                }
                return roles;
            }
        }

        public static IDictionary<CRUD, Claim> ListAllClaims
        {
            get
            {
                var claims = new Dictionary<CRUD, Claim>();
                foreach (var permission in Enum.GetNames(typeof(CRUD)))
                {
                    var result = (CRUD)Enum.Parse(typeof(CRUD), permission);
                    claims.Add(result, new Claim(CustomClaimTypes.DefaultPermission, $"{NAME}.{permission.ToLower()}"));
                }

                //claims.Add(CRUD.Create, new Claim(CustomClaimTypes.DefaultPermission, $"{DeliveryPolicy.ExecuteDelivery.ToString().ToLower()}"));
                return claims;
            }
        }

        public static IList<string> ListClaimsAuthorizations { get { return ListAllClaims.Select(x => x.Value.Value).ToList(); } }
    }

    public enum CRUD
    {
        Create,
        Read,
        Update,
        Delete,
        Index
    }
}
