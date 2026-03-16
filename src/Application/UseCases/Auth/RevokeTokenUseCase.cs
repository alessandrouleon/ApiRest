using APIRest.Application.Interfaces;
using APIRest.Domain.Exceptions;
using APIRest.Domain.Interfaces;

namespace APIRest.Application.UseCases.Auth;

public class RevokeTokenUseCase
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public RevokeTokenUseCase(IRefreshTokenRepository refreshTokenRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task ExecuteAsync(string incomingToken)
    {
        var tokenHash = ITokenService.Hash(incomingToken);
        var stored = await _refreshTokenRepository.GetByTokenHashAsync(tokenHash);

        if (stored is null || !stored.IsActive)
            throw new InvalidCredentialsException();

        stored.Revoke();
        await _refreshTokenRepository.UpdateAsync(stored);
    }
}
