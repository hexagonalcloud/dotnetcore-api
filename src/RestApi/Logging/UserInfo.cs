using System.Collections.Generic;
using System.Security.Claims;

namespace RestApi.Logging
{
    public class UserInfo
    {
        public UserInfo(ClaimsPrincipal claimsPrincipal)
        {
            // TODO: review
            Claims = claimsPrincipal.Claims;
            if (claimsPrincipal.Identity != null)
            {
                UserName = claimsPrincipal.Identity.Name;
            }
        }

        public string UserName { get; }

        public IEnumerable<Claim> Claims { get; }
    }
}
