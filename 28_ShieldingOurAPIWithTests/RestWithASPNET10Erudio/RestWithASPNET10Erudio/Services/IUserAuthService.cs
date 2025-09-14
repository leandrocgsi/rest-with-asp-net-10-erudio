using RestWithASPNET10Erudio.Data.DTO.V1;
using RestWithASPNET10Erudio.Model;

namespace RestWithASPNET10Erudio.Services
{
    public interface IUserAuthService
    {
        User? FindByUsername(string username);
        User Create(AccountCredentialsDTO dto);
        bool RevokeToken(string username);
        User Update (User user);
    }
}
