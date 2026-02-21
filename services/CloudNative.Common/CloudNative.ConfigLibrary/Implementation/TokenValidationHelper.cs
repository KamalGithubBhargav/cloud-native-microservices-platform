using CloudNative.ConfigLibrary.Constants;
using CloudNative.ConfigLibrary.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CloudNative.ConfigLibrary.Implementation
{
    // TokenValidationHelper.cs in Class Library
    public class TokenValidationHelper : ITokenValidationHelper
    {

        private readonly IConfiguration _config;

        public TokenValidationHelper(IConfiguration config)
        {
            _config = config;
        }

        public void AttachUserToContext(HttpContext context, string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config[JwtConstant.Key]!);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _config[JwtConstant.Issuer],
                    ValidAudience = _config[JwtConstant.Audience],
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.Name).Value;

                context.Items["UserId"] = userId;
            }
            catch
            {
                // Token invalid or expired
            }
        }
    }
}
