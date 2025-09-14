using System.Security.Claims;

namespace RestWithASPNET10Erudio.Auth.Contract
{
    public interface ITokenGenerator
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
