using APIRest.Application.DTOs.Requests;
using APIRest.Application.DTOs.Responses;
using APIRest.Application.Interfaces;
using APIRest.Application.Mappers;
using APIRest.Domain.Exceptions;
using APIRest.Domain.Factories;
using APIRest.Domain.Interfaces;

namespace APIRest.Application.UseCases.CreateUser;

public class CreateUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserUseCase(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserResponse> ExecuteAsync(CreateUserRequest request)
    {
        var existing = await _userRepository.GetByEmailAsync(request.Email);
        if (existing is not null)
            throw new EmailAlreadyInUseException(request.Email);

        var passwordHash = _passwordHasher.Hash(request.Password);
        var user = UserFactory.Create(request.Name, request.Email, passwordHash);

        await _userRepository.AddAsync(user);

        return UserMapper.ToResponse(user);
    }
}
