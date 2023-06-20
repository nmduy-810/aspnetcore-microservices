using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Contracts.Identity;
using Microsoft.IdentityModel.Tokens;
using Shared.Configurations;
using Shared.DTOs.Identity;

namespace Infrastructure.Identity;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;

    public TokenService(JwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings ?? throw new ArgumentNullException(nameof(jwtSettings));
    }

    public TokenResponse GetToken(TokenRequest request)
    {
        var token = GenerateJwt();
        var result = new TokenResponse(token);
        return result;
    }

    private string GenerateJwt() => GenerateEncryptedToken(GetSigningCredential());

    private string GenerateEncryptedToken(SigningCredentials signingCredentials)
    {
        var token = new JwtSecurityToken(expires: DateTime.Now.AddMinutes(30),
                                         signingCredentials: signingCredentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private SigningCredentials GetSigningCredential()
    {
        byte[] secret = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
    }
}