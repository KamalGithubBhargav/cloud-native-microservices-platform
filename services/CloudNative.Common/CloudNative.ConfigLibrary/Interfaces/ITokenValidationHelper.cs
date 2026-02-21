using Microsoft.AspNetCore.Http;

namespace CloudNative.ConfigLibrary.Interfaces
{
    public interface ITokenValidationHelper
    {
        public void AttachUserToContext(HttpContext context, string token);
    }
}
