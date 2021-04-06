using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ReviewItServer.DTOs;
using ReviewItServer.Models;
using ReviewItServer.Utility;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ReviewItServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticateController : Controller
    {
        private UserManager<User> userManager;

        public AuthenticateController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                List<Claim> authClaims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var userRoles = await userManager.GetRolesAsync(user);
                foreach(var role in userRoles)
                {
                    authClaims.Add(new Claim("role", role));
                }
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("X9DxG4abknX3JtT6"));
                var token = new JwtSecurityToken(
                    expires: DateTime.Now.AddDays(1),
                    claims: authClaims.ToArray(),
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    ) ;
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        [Authorize]
        [HttpGet]
        [Route("isuser")]
        public IActionResult IsUser()
        {
            return Ok();
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        [Route("isadmin")]
        public IActionResult IsAdmin()
        {
            return Ok();
        }
    }
}
