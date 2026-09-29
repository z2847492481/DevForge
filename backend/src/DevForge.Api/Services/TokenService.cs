using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DevForge.Api.Entities;
using DevForge.Api.Security;
using Microsoft.IdentityModel.Tokens;

namespace DevForge.Api.Services;

public class TokenService : ITokenService
{
    private readonly JwtSettings _settings;

    public TokenService(Microsoft.Extensions.Options.IOptions<JwtSettings> options)
    {
        _settings = options.Value;
    }

    public (string Token, long ExpiresAtUnixSeconds) CreateToken(User user)
    {
        var expiresAt = DateTimeOffset.UtcNow.AddMinutes(_settings.ExpireMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Name, user.FullName ?? user.Username),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiresAt.UtcDateTime,
            signingCredentials: credentials);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return (jwt, expiresAt.ToUnixTimeSeconds());
    }
}