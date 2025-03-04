using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using OrderService.Models;

namespace OrderService.Config
{
    public class Utilities
    {
        /// <summary>
        /// Generates a JWT token for a given user.
        /// </summary>
        /// <param name="user">The user for whom the token is generated.</param>
        /// <param name="jwtSecret">The secret key for signing the token.</param>
        /// <param name="jwtIssuer">The issuer of the token.</param>
        /// <param name="jwtAudience">The audience of the token.</param>
        /// <param name="jwtExpiresIn">The expiration time in minutes.</param>
        /// <returns>A signed JWT token as a string.</returns>
        public string GenerateJwtToken(User user, string jwtSecret, string jwtIssuer, string jwtAudience, int jwtExpiresIn)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwtExpiresIn),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}