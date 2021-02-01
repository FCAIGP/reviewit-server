using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Security.Principal;

namespace Company_Reviewing_System.Utility
{
    public static class Helpers
    {
        public static string GetId(this IIdentity identity)
        {
            return ((ClaimsIdentity)identity).FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
