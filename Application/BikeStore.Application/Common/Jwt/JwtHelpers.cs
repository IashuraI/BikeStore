using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BikeStore.Application.Common.Jwt
{
    public static class JwtHelpers
    {
        public static JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, JwtSettings jwtSettings)
        {
            try
            {
                // Get secret key
                var key = System.Text.Encoding.ASCII.GetBytes(jwtSettings.IssuerSigningKey);
                Guid Id = Guid.Empty;

                DateTime expireTime = DateTime.UtcNow.AddDays(1);
                var JWToken = new JwtSecurityToken(issuer: jwtSettings.ValidIssuer, audience: jwtSettings.ValidAudience, claims: claims, notBefore: new DateTimeOffset(DateTime.Now).DateTime, expires: new DateTimeOffset(expireTime).DateTime, signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256));

                return JWToken;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
