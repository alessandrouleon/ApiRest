using APIRest.Application.DTOs.Requests;
using APIRest.Application.DTOs.Responses;
using APIRest.Application.Interfaces;
using APIRest.Application.Mappers;
using APIRest.Domain.Exceptions;
using APIRest.Domain.Interfaces;
using APIRest.Domain.ValueObjects;

namespace APIRest.Application.UseCases.UpdateUser;

public class UpdateUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UpdateUserUseCase(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserResponse> ExecuteAsync(Guid id, UpdateUserRequest request)
    {
        var user = await _userRepository.GetByIdAsync(id)
            ?? throw new UserNotFoundException(id);

        if (!user.IsActive)
            throw new UserInactiveException(id);

        var emailInUse = await _userRepository.GetByEmailAsync(request.Email);
        if (emailInUse is not null && emailInUse.Id != id)
            throw new EmailAlreadyInUseException(request.Email);

        var usernameInUse = await _userRepository.GetByUsernameAsync(request.Username);
        if (usernameInUse is not null && usernameInUse.Id != id)
            throw new UsernameAlreadyInUseException(request.Username);

        user.Update(request.Name, request.Username, new Email(request.Email), request.IsActive);

        if (request.Password is not null)
        {
            var passwordHash = _passwordHasher.Hash(request.Password);
            user.UpdatePassword(passwordHash);
        }

        await _userRepository.UpdateAsync(user);

        return UserMapper.ToResponse(user);
    }
}
