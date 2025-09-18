using Microsoft.IdentityModel.JsonWebTokens;
using RestWithASPNET10Erudio.Auth.Config;
using RestWithASPNET10Erudio.Auth.Contract;
using RestWithASPNET10Erudio.Data.DTO.V1;
using RestWithASPNET10Erudio.Model;
using System.Security.Claims;

namespace RestWithASPNET10Erudio.Services.Impl
{

    public class LoginServiceImpl : ILoginService
    {
        private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

        private readonly IUserAuthService _userAuthService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenService;
        private readonly TokenConfiguration _configurations;

        public LoginServiceImpl(
            IUserAuthService userAuthService,
            IPasswordHasher passwordHasher,
            ITokenGenerator tokenService,
            TokenConfiguration configurations)
        {
            _userAuthService = userAuthService;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _configurations = configurations;
        }

        public TokenDTO ValidateCredentials(UserDTO userDto)
        {
            var user = _userAuthService
                .FindByUsername(userDto.Username);

            if (user == null) return null;
            if (!_passwordHasher.Verify(userDto.Password, user.Password))
                return null;

            return GenerateToken(user);
        }

        public TokenDTO ValidateCredentials(TokenDTO token)
        {
            var principal = _tokenService
                .GetPrincipalFromExpiredToken(token.AccessToken);
            var username = principal.Identity?.Name;

            var user = _userAuthService.FindByUsername(username);
            if (user == null ||
                user.RefreshToken != token.RefreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.Now
                ) 
                return null;
            return GenerateToken(user, principal.Claims);
        }

        public AccountCredentialsDTO Create(AccountCredentialsDTO dto)
        {
            var user = _userAuthService
                .Create(dto);

            return new AccountCredentialsDTO
            {
                Username = user.Username,
                Fullname = user.FullName,
                Password = "************"
            };
        }

        public bool RevokeToken(string username)
        {
            return _userAuthService
                .RevokeToken(username);
        }

        private TokenDTO GenerateToken(User user,
            IEnumerable<Claim>? existingClaims = null)
        {
            var claims = existingClaims?.ToList() ??
                // new List<Claim>
                [
                    new Claim(JwtRegisteredClaimNames.Jti,
                        Guid.NewGuid().ToString("N")),

                    new Claim(JwtRegisteredClaimNames.UniqueName,
                        user.Username),
                ];

            var accessToken = _tokenService
                .GenerateAccessToken(claims);

            var refreshToken = _tokenService
                .GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now
                .AddDays(_configurations.DaysToExpiry);
            
            _userAuthService.Update(user);

            var createdDate = DateTime.Now;
            var expirationDate = createdDate
                .AddMinutes(_configurations.Minutes);

            return new TokenDTO
            {
                Authenticated = true,
                Created = createdDate.ToString(DATE_FORMAT),
                Expiration = expirationDate
                    .ToString(DATE_FORMAT),

                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
