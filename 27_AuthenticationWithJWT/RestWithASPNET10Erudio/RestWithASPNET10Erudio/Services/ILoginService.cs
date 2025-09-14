using RestWithASPNET10Erudio.Data.DTO.V1;

namespace RestWithASPNET10Erudio.Services
{
    public interface ILoginService
    {
        TokenDTO? ValidateCredentials(UserDTO user);
        TokenDTO? ValidateCredentials(TokenDTO token);
        bool RevokeToken(string username);
        AccountCredentialsDTO Create(AccountCredentialsDTO user);
    }
}
