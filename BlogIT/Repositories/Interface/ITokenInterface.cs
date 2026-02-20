using Microsoft.AspNetCore.Identity;

namespace BlogIT.Repositories.Interface
{
    public interface ITokenInterface
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
