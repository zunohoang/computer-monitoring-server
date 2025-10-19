using ComputerMonitoringServerAPI.Data;
using ComputerMonitoringServerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ComputerMonitoringServerAPI.Repositories
{
    public interface IProfileRepository
    {
        Task<User?> GetProfileAsync(long userId);
        Task UpdateProfileAsync(User user);
        Task SaveChangesAsync();
    }

    public class ProfileRepository : IProfileRepository
    {
        private readonly AppDbContext _context;

        public ProfileRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetProfileAsync(long userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task UpdateProfileAsync(User user)
        {
            _context.Users.Update(user);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
