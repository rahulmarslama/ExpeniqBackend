using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Expendiq.Application.Helpers
{
    public static class IdentityHelper
    {
        public static int GetUserId(this IIdentity identity)
        {
            if (identity is not ClaimsIdentity claimsIdentity)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            Claim claim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);
            if (claim == null || int.Parse(claim.Value) <= 0)
            {
                throw new InvalidOperationException("Invalid user id claim");
            }
            else
            {
                return int.Parse(claim.Value);
            }
        }
    }
}
