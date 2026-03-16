using APIRest.Application.DTOs.Responses;
using APIRest.Application.Interfaces;
using APIRest.Domain.Entities;
using APIRest.Domain.Exceptions;
using APIRest.Domain.Interfaces;

namespace APIRest.Application.UseCases.Auth;

public class RefreshTokenUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ITokenService _tokenService;

    public RefreshTokenUseCase(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> ExecuteAsync(string incomingToken)
    {
        var tokenHash = ITokenService.Hash(incomingToken);
        var stored = await _refreshTokenRepository.GetByTokenHashAsync(tokenHash);

        if (stored is null || !stored.IsActive)
            throw new InvalidCredentialsException();

        var user = await _userRepository.GetByIdAsync(stored.UserId);

        if (user is null || !user.IsActive)
            throw new InvalidCredentialsException();

        // Revoke used token (rotation)
        stored.Revoke();
        await _refreshTokenRepository.UpdateAsync(stored);

        // Issue new token pair
        var accessToken = _tokenService.GenerateAccessToken(user);
        var (refreshTokenPlain, refreshTokenHash) = _tokenService.GenerateRefreshToken();

        var newRefreshToken = new RefreshToken(
            user.Id,
            refreshTokenHash,
            _tokenService.GetRefreshTokenExpiration());

        await _refreshTokenRepository.AddAsync(newRefreshToken);

        return new AuthResponse(
            accessToken,
            refreshTokenPlain,
            _tokenService.GetAccessTokenExpiration(),
            newRefreshToken.ExpiresAt);
    }
}
