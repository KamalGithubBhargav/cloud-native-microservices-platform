using CloudNative.Identity.Core.Entities;

namespace CloudNative.Identity.Core.Repositories.AuthServices
{
    public interface IUserService
    {
        public Task<UserEntity> ValidateUserAsync(string userName, string password);
    }
}
