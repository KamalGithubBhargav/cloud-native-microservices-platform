using CloudNative.Identity.Application.DTOs.Auth;
using MediatR;

namespace CloudNative.Identity.Application.Features.Auth.Commands
{
    public class RefreshTokenCommand : IRequest<AuthResponse>
    {
    }
}
