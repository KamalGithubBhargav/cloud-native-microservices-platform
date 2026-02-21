using CloudNative.ConfigLibrary.Constants;
using CloudNative.ConfigLibrary.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace CloudNative.Identity.Api.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;
        private readonly ITokenValidationHelper _tokenValidationHelper;
        public JwtMiddleware(RequestDelegate next
            , IConfiguration config
            , ITokenValidationHelper tokenValidationHelper)
        {
            _next = next;
            _config = config;
            _tokenValidationHelper = tokenValidationHelper;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();

            if (path != null && (
                path.Contains("/swagger")
            ))
            {
                await _next(context);
                return;
            }

            var endpoint = context.GetEndpoint();

            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                await _next(context);
                return;
            }

            var token = context.Request.Headers[HeaderConstant.Authorization].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                try
                {
                    _tokenValidationHelper.AttachUserToContext(context, token);
                }
                catch
                {
                    // Token invalid → return 401 immediately
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync(AuthMessageConstant.ExpiredToken);
                    return;
                }
            }
            else
            {
                // No token → return 401 immediately
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync(AuthMessageConstant.AuthorizationTokenMissing);
                return;
            }

            await _next(context);
        }

    }
}
