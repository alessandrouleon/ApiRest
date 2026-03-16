using APIRest.API.Common;
using APIRest.Application.DTOs.Requests;
using APIRest.Application.DTOs.Responses;
using APIRest.Application.UseCases.Auth;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace APIRest.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly LoginUseCase _login;
    private readonly RefreshTokenUseCase _refreshToken;
    private readonly RevokeTokenUseCase _revokeToken;
    private readonly IValidator<LoginRequest> _loginValidator;

    public AuthController(
        LoginUseCase login,
        RefreshTokenUseCase refreshToken,
        RevokeTokenUseCase revokeToken,
        IValidator<LoginRequest> loginValidator)
    {
        _login = login;
        _refreshToken = refreshToken;
        _revokeToken = revokeToken;
        _loginValidator = loginValidator;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var validation = await _loginValidator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => e.ErrorMessage);
            return BadRequest(ApiResponse<object>.Fail("Validation failed.", errors));
        }

        var result = await _login.ExecuteAsync(request);
        return Ok(ApiResponse<AuthResponse>.Ok(result, "Login successful."));
    }

    [HttpPost("refresh")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        var result = await _refreshToken.ExecuteAsync(request.RefreshToken);
        return Ok(ApiResponse<AuthResponse>.Ok(result, "Token refreshed successfully."));
    }

    [HttpPost("revoke")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Revoke([FromBody] RefreshTokenRequest request)
    {
        await _revokeToken.ExecuteAsync(request.RefreshToken);
        return Ok(ApiResponse<object>.Ok(null!, "Token revoked successfully."));
    }
}
