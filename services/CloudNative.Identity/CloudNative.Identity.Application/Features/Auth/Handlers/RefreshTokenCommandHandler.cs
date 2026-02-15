using CloudNative.Identity.Application.DTOs.Auth;
using CloudNative.Identity.Application.Features.Auth.Commands;
using CloudNative.Identity.Core.Caching;
using CloudNative.Identity.Core.Repositories.AuthServices;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CloudNative.Identity.Application.Features.Auth.Handlers
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponse>
    {
        private readonly IRefreshTokenStore _refreshTokenStore;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RefreshTokenCommandHandler(
            IRefreshTokenStore refreshTokenStore,
            ITokenService tokenService,
            IHttpContextAccessor httpContextAccessor)
        {
            _refreshTokenStore = refreshTokenStore;
            _tokenService = tokenService;
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

            var newRefreshToken = _tokenService.GenerateRefreshToken();
            await _refreshTokenStore.StoreAsync(newRefreshToken, userId, TimeSpan.FromDays(7));

            context.Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            var newAccessToken = _tokenService.GenerateAccessToken(userId);

            return new AuthResponse { AccessToken = newAccessToken };
        }

    }

}
