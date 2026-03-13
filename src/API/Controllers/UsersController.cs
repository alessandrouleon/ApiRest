using APIRest.API.Common;
using APIRest.Application.DTOs.Requests;
using APIRest.Application.DTOs.Responses;
using APIRest.Application.UseCases.CreateUser;
using APIRest.Application.UseCases.DeleteUser;
using APIRest.Application.UseCases.FindAllUsers;
using APIRest.Application.UseCases.FindUserById;
using APIRest.Application.UseCases.UpdateUser;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace APIRest.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly CreateUserUseCase _createUser;
    private readonly FindAllUsersUseCase _findAllUsers;
    private readonly FindUserByIdUseCase _findUserById;
    private readonly UpdateUserUseCase _updateUser;
    private readonly DeleteUserUseCase _deleteUser;
    private readonly IValidator<CreateUserRequest> _createValidator;
    private readonly IValidator<UpdateUserRequest> _updateValidator;

    public UsersController(
        CreateUserUseCase createUser,
        FindAllUsersUseCase findAllUsers,
        FindUserByIdUseCase findUserById,
        UpdateUserUseCase updateUser,
        DeleteUserUseCase deleteUser,
        IValidator<CreateUserRequest> createValidator,
        IValidator<UpdateUserRequest> updateValidator)
    {
        _createUser = createUser;
        _findAllUsers = findAllUsers;
        _findUserById = findUserById;
        _updateUser = updateUser;
        _deleteUser = deleteUser;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var validation = await _createValidator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => e.ErrorMessage);
            return BadRequest(ApiResponse<object>.Fail("Validation failed.", errors));
        }

        var user = await _createUser.ExecuteAsync(request);
        return CreatedAtAction(
            nameof(GetById),
            new { id = user.Id },
            ApiResponse<UserResponse>.Ok(user, "User created successfully."));
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var users = await _findAllUsers.ExecuteAsync();
        return Ok(ApiResponse<IEnumerable<UserResponse>>.Ok(users));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _findUserById.ExecuteAsync(id);
        return Ok(ApiResponse<UserResponse>.Ok(user));
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequest request)
    {
        var validation = await _updateValidator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => e.ErrorMessage);
            return BadRequest(ApiResponse<object>.Fail("Validation failed.", errors));
        }

        var user = await _updateUser.ExecuteAsync(id, request);
        return Ok(ApiResponse<UserResponse>.Ok(user, "User updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _deleteUser.ExecuteAsync(id);
        return Ok(ApiResponse<object>.Ok(null!, "User deleted successfully."));
    }
}
