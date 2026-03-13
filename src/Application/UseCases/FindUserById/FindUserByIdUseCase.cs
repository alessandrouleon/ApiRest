using APIRest.Application.DTOs.Responses;
using APIRest.Application.Mappers;
using APIRest.Domain.Exceptions;
using APIRest.Domain.Interfaces;

namespace APIRest.Application.UseCases.FindUserById;

public class FindUserByIdUseCase
{
    private readonly IUserRepository _userRepository;

    public FindUserByIdUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponse> ExecuteAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id)
            ?? throw new UserNotFoundException(id);

        return UserMapper.ToResponse(user);
    }
}
