namespace CloudNative.Identity.Core.Constants
{
    public class JwtConstant
    {
        public const string Key = "Jwt:Key";
        public const string Issuer = "Jwt:Issuer";
        public const string Audience = "Jwt:Audience";
        public const string RefreshTokenExpirationDays = "Jwt:RefreshTokenExpirationDays";
        public const string AccessTokenExpirationMinutes = "Jwt:AccessTokenExpirationMinutes";
        public const string CookiesRefreshToken = "refreshToken";
        public const int AccessTokenExpMins = 15;
        public const int RefreshTokenExpDays = 1;


    }
}
