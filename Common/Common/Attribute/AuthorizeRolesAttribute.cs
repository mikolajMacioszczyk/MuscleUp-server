using Common.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Common.Attribute
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params string[] roles) : base()
        {
            Roles = string.Join(",", roles);
        }

        public AuthorizeRolesAttribute(params RoleType[] roles) : base()
        {
            Roles = string.Join(",", roles.Select(r => r.ToString()));
        }
    }
}
