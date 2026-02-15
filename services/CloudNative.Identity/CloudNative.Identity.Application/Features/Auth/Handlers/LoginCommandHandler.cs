using CloudNative.Identity.Application.DTOs.Auth;
using CloudNative.Identity.Application.Features.Auth.Commands;
using CloudNative.Identity.Core.Caching;
using CloudNative.Identity.Core.Repositories.AuthServices;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CloudNative.Identity.Application.Features.Auth.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
    {
        private readonly IUserService _userService;
        private readonly IRefreshTokenStore _refreshTokenStore;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(
            IUserService userService,
            IRefreshTokenStore refreshTokenStore,
            ITokenService tokenService,
            IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _refreshTokenStore = refreshTokenStore;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userService.ValidateUserAsync(request.login.UserName, request.login.Password);
            if (user == null)
                throw new UnauthorizedAccessException();

            var accessToken = _tokenService.GenerateAccessToken(user.Id.ToString());
            var refreshToken = _tokenService.GenerateRefreshToken();

            await _refreshTokenStore.StoreAsync(refreshToken, user.Id.ToString(), TimeSpan.FromDays(7));

            _httpContextAccessor.HttpContext!.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return new AuthResponse { AccessToken = accessToken };
        }

    }
}



