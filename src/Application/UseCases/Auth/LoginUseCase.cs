using APIRest.Application.DTOs.Requests;
using APIRest.Application.DTOs.Responses;
using APIRest.Application.Interfaces;
using APIRest.Domain.Entities;
using APIRest.Domain.Exceptions;
using APIRest.Domain.Interfaces;

namespace APIRest.Application.UseCases.Auth;

public class LoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public LoginUseCase(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> ExecuteAsync(LoginRequest request)
    {
        var user = await ResolveUserAsync(request.Identifier);

        if (user is null || !user.IsActive)
            throw new InvalidCredentialsException();

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new InvalidCredentialsException();

        return await IssueTokensAsync(user);
    }

    private async Task<User?> ResolveUserAsync(string identifier)
    {
        // Try email first (contains '@'), then username
        if (identifier.Contains('@'))
            return await _userRepository.GetByEmailAsync(identifier);

        var byUsername = await _userRepository.GetByUsernameAsync(identifier);
        if (byUsername is not null) return byUsername;

        // Fallback: try as email even without '@' (edge cases)
        return await _userRepository.GetByEmailAsync(identifier);
    }

    private async Task<AuthResponse> IssueTokensAsync(User user)
    {
        var accessToken = _tokenService.GenerateAccessToken(user);
        var (refreshTokenPlain, refreshTokenHash) = _tokenService.GenerateRefreshToken();

        var refreshToken = new RefreshToken(
            user.Id,
            refreshTokenHash,
            _tokenService.GetRefreshTokenExpiration());

        await _refreshTokenRepository.AddAsync(refreshToken);

        return new AuthResponse(
            accessToken,
            refreshTokenPlain,
            _tokenService.GetAccessTokenExpiration(),
            refreshToken.ExpiresAt);
    }
}
