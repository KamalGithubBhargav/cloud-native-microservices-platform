using CloudNative.Identity.Application.DTOs.Auth;
using MediatR;

namespace CloudNative.Identity.Application.Features.Auth.Commands
{

    public class LoginCommand(LoginDto login) : IRequest<AuthResponse>
    {
        public LoginDto login { get; set; } = login;
    }
}
