using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ReviewItServer.Tests
{
    public class MockFactory
    {
        public static ClaimsPrincipal MockUserPrincipal(string id)
        {
            return new ClaimsPrincipal(
                new ClaimsIdentity[] {
                    new ClaimsIdentity(
                        new Claim[] {
                            new Claim(ClaimTypes.NameIdentifier, id)
                        }
                    )
                }
            );
        }
    }
}
