using CloudNative.Identity.Core.Entities;
using CloudNative.Identity.Core.Repositories.AuthServices;

namespace CloudNative.Identity.Infrastructure.Persistence.Auth
{
    public class UserService : IUserService
    {
        private static readonly List<UserEntity> Users = new()
        {
            new UserEntity { Id = 1, UserName = "alice", Password = "Password123!" },
            new UserEntity { Id = 2, UserName = "bob", Password = "Secret456!" },
            new UserEntity { Id = 3, UserName = "charlie", Password = "MyPass789!" }
        };

        public async Task<UserEntity> ValidateUserAsync(string userName, string password)
        {
            await Task.Delay(50);

            // Case-insensitive username match
            var user = Users.FirstOrDefault(u =>
                u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase)
                && u.Password == password);

            return user ?? null!;
        }
    }
}
