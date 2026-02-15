namespace CloudNative.Identity.Core.Repositories.AuthServices
{
    public interface ITokenService
    {
        string GenerateAccessToken(string userId);
        string GenerateRefreshToken();
    }
}
