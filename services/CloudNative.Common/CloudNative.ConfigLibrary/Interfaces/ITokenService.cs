namespace CloudNative.ConfigLibrary.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(string userId);
        string GenerateRefreshToken();
    }
}
