using SideBySideAPI.Models;
using SideBySideAPI.Models.DTOs;

namespace SideBySideAPI.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(string id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User> CreateUserAsync(RegisterUserDTO registerDto);
        Task<User?> AuthenticateAsync(string email, string password);
        Task<User> UpdateUserAsync(string userId, UpdateUserDTO updateDto);
        Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<bool> DeleteUserAsync(string userId);
        Task<UserPreferences> UpdateUserPreferencesAsync(string userId, UserPreferencesDTO preferencesDto);
    }
} 