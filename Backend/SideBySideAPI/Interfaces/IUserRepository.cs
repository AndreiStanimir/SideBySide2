using SideBySideAPI.Models;

namespace SideBySideAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(string id);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByUsernameAsync(string username);
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(string id, User user);
        Task DeleteAsync(string id);
    }
} 