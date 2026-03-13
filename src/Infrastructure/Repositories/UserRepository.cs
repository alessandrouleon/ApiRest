using APIRest.Domain.Entities;
using APIRest.Domain.Interfaces;
using APIRest.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace APIRest.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.DeletedAt == null);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.AsNoTracking().Where(u => u.DeletedAt == null).ToListAsync();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var normalized = email.ToLowerInvariant();
        return await _context.Users.FirstOrDefaultAsync(u => u.Email.Value == normalized && u.DeletedAt == null);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        user.SoftDelete();
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}
