using BikeStore.Application.Common.Jwt;
using BikeStore.Domain.Constants;
using BikeStore.Infrastructure.EntityFramework.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BikeStore.WebApi.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly JwtSettings _jwtSettings;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager,
            JwtSettings jwtSettings)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtSettings = jwtSettings;
        }

        [HttpPost]
        public async Task RegisterUser(string username, string password, int? customerId = null, int? staffId = null)
        {
            var user = new User { Id = Guid.NewGuid().ToString(), UserName = username, Email = username, CustomerId = customerId };

            if (staffId != null)
            {
                IdentityResult staffResult = await _userManager.CreateAsync(user, password);

                if (staffResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, RoleConstants.StaffRoleName);
                }
            }
            else if (customerId != null)
            {
                IdentityResult result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, RoleConstants.CustomerRoleName);
                }
            }
        }

        [HttpPost("CreateTokenForExistingUser")]
        public async Task<string> CreateTokenForExistingUser(string username, string password)
        {
            User? user = await _userManager.FindByEmailAsync(username);

            if(user != null && await _userManager.CheckPasswordAsync(user, password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                if (userRoles.Contains(RoleConstants.CustomerRoleName))
                {
                    authClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.CustomerId.ToString()!));
                }

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                SecurityToken token = JwtHelpers.GenerateAccessToken(authClaims, _jwtSettings);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            else
            {
                return "User was not identified";
            }
        }
    }
}
