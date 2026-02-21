using CloudNative.ConfigLibrary.Constants;
using CloudNative.ConfigLibrary.Interfaces;
using CloudNative.Identity.Application.DTOs.Auth;
using CloudNative.Identity.Application.Features.Auth.Commands;
using CloudNative.Identity.Core.Caching;
using CloudNative.Identity.Core.Repositories.AuthServices;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CloudNative.Identity.Application.Features.Auth.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
    {
        private readonly IUserService _userService;
        private readonly IRefreshTokenStore _refreshTokenStore;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;

        public LoginCommandHandler(
            IUserService userService,
            IRefreshTokenStore refreshTokenStore,
            ITokenService tokenService,
            IConfiguration config,
            IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _refreshTokenStore = refreshTokenStore;
            _tokenService = tokenService;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userService.ValidateUserAsync(request.login.UserName, request.login.Password);
            if (user == null)
                throw new UnauthorizedAccessException();

            var accessToken = _tokenService.GenerateAccessToken(user.Id.ToString());
            var refreshToken = _tokenService.GenerateRefreshToken();
            var expires = _config[JwtConstant.RefreshTokenExpirationDays];
            int dayExp = 0;
            bool isDayExp = !string.IsNullOrEmpty(expires) ?
                 int.TryParse(expires, out dayExp) : false;
            dayExp = isDayExp ? dayExp : JwtConstant.RefreshTokenExpDays;

            await _refreshTokenStore.StoreAsync(refreshToken, user.Id.ToString(), TimeSpan.FromDays(dayExp));

            _httpContextAccessor.HttpContext!.Response.Cookies.Append(JwtConstant.CookiesRefreshToken, refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(dayExp)
            });

            return new AuthResponse { AccessToken = accessToken };
        }

    }
}



