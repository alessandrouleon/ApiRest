using APIRest.Domain.Exceptions;
using APIRest.Domain.Interfaces;

namespace APIRest.Application.UseCases.DeleteUser;

public class DeleteUserUseCase
{
    private readonly IUserRepository _userRepository;

    public DeleteUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task ExecuteAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id)
            ?? throw new UserNotFoundException(id);

        if (!user.IsActive)
            throw new UserInactiveException(id);

        await _userRepository.DeleteAsync(user);
    }
}
