using API.Entity;

namespace API.Interface
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}