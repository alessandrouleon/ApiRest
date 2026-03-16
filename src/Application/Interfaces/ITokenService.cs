using System.Security.Cryptography;
using System.Text;
using APIRest.Domain.Entities;

namespace APIRest.Application.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    DateTime GetAccessTokenExpiration();
    (string token, string hash) GenerateRefreshToken();
    DateTime GetRefreshTokenExpiration();

    // Shared hashing utility — same algorithm used in GenerateRefreshToken
    static string Hash(string token)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
