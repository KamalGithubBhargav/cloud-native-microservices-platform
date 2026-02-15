using CloudNative.Identity.Core.Caching;
using StackExchange.Redis;

namespace CloudNative.Identity.Infrastructure.Caching
{
    public class RefreshTokenStore : IRefreshTokenStore
    {
        private readonly IDatabase _redis;

        public RefreshTokenStore(IConnectionMultiplexer redis)
        {
            _redis = redis.GetDatabase();
        }

        public async Task StoreAsync(string refreshToken, string userId, TimeSpan ttl)
        {
            await _redis.StringSetAsync(
                $"refresh:{refreshToken}",
                userId,
                ttl
            );
        }

        public async Task<string?> GetUserIdAsync(string refreshToken)
        {
            return await _redis.StringGetAsync($"refresh:{refreshToken}");
        }

        public async Task RevokeAsync(string refreshToken)
        {
            await _redis.KeyDeleteAsync($"refresh:{refreshToken}");
        }
    }
}
