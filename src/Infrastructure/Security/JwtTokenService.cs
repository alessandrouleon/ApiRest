using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using APIRest.Application.Interfaces;
using APIRest.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace APIRest.Infrastructure.Security;

public class JwtTokenService : ITokenService
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _accessTokenExpirationMinutes;
    private readonly int _refreshTokenExpirationDays;

    public JwtTokenService(IConfiguration configuration)
    {
        var section = configuration.GetSection("Jwt");
        _secretKey = section["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is not configured.");
        _issuer = section["Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured.");
        _audience = section["Audience"] ?? throw new InvalidOperationException("JWT Audience is not configured.");
        _accessTokenExpirationMinutes = int.Parse(section["AccessTokenExpirationMinutes"] ?? "15");
        _refreshTokenExpirationDays = int.Parse(section["RefreshTokenExpirationDays"] ?? "7");
    }

    public string GenerateAccessToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email.Value),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: GetAccessTokenExpiration(),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public DateTime GetAccessTokenExpiration()
        => DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes);

    public (string token, string hash) GenerateRefreshToken()
    {
        var tokenBytes = RandomNumberGenerator.GetBytes(64);
        var token = Convert.ToBase64String(tokenBytes);
        var hash = ITokenService.Hash(token);
        return (token, hash);
    }

    public DateTime GetRefreshTokenExpiration()
        => DateTime.UtcNow.AddDays(_refreshTokenExpirationDays);
}
