using CloudNative.Identity.Core.Constants;
using CloudNative.Identity.Core.Repositories.AuthServices;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CloudNative.Identity.Infrastructure.Persistence.Auth
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateAccessToken(string userId)
        {
            var claims = new[] { new Claim(ClaimTypes.Name, userId) };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config[JwtConstant.Key]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = _config[JwtConstant.AccessTokenExpirationMinutes];

            int minsExp = 0;
            bool isMinExp = !string.IsNullOrEmpty(expires) ? 
                 int.TryParse(expires, out minsExp) : false;
            minsExp = isMinExp ? minsExp : JwtConstant.AccessTokenExpMins;

            var token = new JwtSecurityToken(
                issuer: _config[JwtConstant.Issuer],
                audience: _config[JwtConstant.Audience],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(minsExp),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}



