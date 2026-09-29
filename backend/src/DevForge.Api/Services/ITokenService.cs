using DevForge.Api.Entities;

namespace DevForge.Api.Services;

public interface ITokenService
{
    (string Token, long ExpiresAtUnixSeconds) CreateToken(User user);
}