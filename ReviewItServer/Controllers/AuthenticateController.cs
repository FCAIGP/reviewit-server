using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public AuthenticateController(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Allows users to log into the system
        /// </summary>
        /// <param name="model">The data of the user</param>
        /// <response code="200">Returned if user is logged in successfully</response>
        /// <response code="401">Returned if user is not authorized (No user with given credentials was found)</response>
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                List<Claim> authClaims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var userRoles = await _userManager.GetRolesAsync(user);
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

        /// <summary>
        /// Allows non-users to register in the system
        /// </summary>
        /// <param name="dto">The data of the user</param>
        /// <response code="200">Returned if user registered successfully in the system</response>
        /// <response code="400">Returned if failure happens during registeration</response>
        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] UserDto dto)
        {
            if (ModelState.IsValid)
            {
                User user = _mapper.Map<User>(dto);
                //TODO: recieve a hash of the password instead of the password itself.
                var result = await _userManager.CreateAsync(user, dto.Password);
                if (result.Succeeded)
                {
                    return Ok($"User {user.UserName} created successfully!");
                }
                return BadRequest(result.Errors.Select(v => v.Description));
            }
            return BadRequest("Something failed!");
        }

        /// <summary>
        /// Checks if user is logged in (requires User priveleges)
        /// </summary>
        /// <response code="200">Returned if User is logged in</response>
        /// <response code="401">Returned if User is not logged in</response>
        [Authorize]
        [HttpGet("isUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult IsUser()
        {
            return Ok();
        }

        /// <summary>
        /// Checks if user is admin (requires Admin priveleges)
        /// </summary>
        /// <response code ="200">Returned if user is logged in and Admin</response>
        /// <response code ="401">Returned if user is not Admin</response>
        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        [Route("isAdmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult IsAdmin()
        {
            return Ok();
        }
    }
}
