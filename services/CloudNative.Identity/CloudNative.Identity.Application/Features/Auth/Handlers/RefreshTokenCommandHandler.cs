using CloudNative.Identity.Application.DTOs.Auth;
using CloudNative.Identity.Application.Features.Auth.Commands;
using CloudNative.Identity.Core.Caching;
using CloudNative.Identity.Core.Constants;
using CloudNative.Identity.Core.Repositories.AuthServices;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CloudNative.Identity.Application.Features.Auth.Handlers
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponse>
    {
        private readonly IRefreshTokenStore _refreshTokenStore;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;

        public RefreshTokenCommandHandler(
            IRefreshTokenStore refreshTokenStore,
            ITokenService tokenService,
            IConfiguration config,
            IHttpContextAccessor httpContextAccessor)
        {
            _refreshTokenStore = refreshTokenStore;
            _tokenService = tokenService;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var context = _httpContextAccessor.HttpContext!;
            if (!context.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
                throw new UnauthorizedAccessException();

            var userId = await _refreshTokenStore.GetUserIdAsync(refreshToken!);
            if (userId == null)
                throw new UnauthorizedAccessException();

            await _refreshTokenStore.RevokeAsync(refreshToken!);
            
            var expires = _config[JwtConstant.RefreshTokenExpirationDays];
            int dayExp = 0;
            bool isDayExp = !string.IsNullOrEmpty(expires) ?
                 int.TryParse(expires, out dayExp) : false;
            dayExp = isDayExp ? dayExp : JwtConstant.RefreshTokenExpDays;

            var newRefreshToken = _tokenService.GenerateRefreshToken();
            await _refreshTokenStore.StoreAsync(newRefreshToken, userId, TimeSpan.FromDays(dayExp));

            context.Response.Cookies.Append(JwtConstant.CookiesRefreshToken, newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(dayExp)
            });

            var newAccessToken = _tokenService.GenerateAccessToken(userId);

            return new AuthResponse { AccessToken = newAccessToken };
        }

    }

}
