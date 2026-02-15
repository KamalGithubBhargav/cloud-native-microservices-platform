namespace CloudNative.Identity.Core.Caching
{
    public interface IRefreshTokenStore
    {
        Task StoreAsync(string refreshToken, string userId, TimeSpan ttl);
        Task<string?> GetUserIdAsync(string refreshToken);
        Task RevokeAsync(string refreshToken);
    }
}
