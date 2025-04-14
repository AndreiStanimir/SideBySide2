using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using SideBySideAPI.Interfaces;
using SideBySideAPI.Models;
using SideBySideAPI.Models.DTOs;

namespace SideBySideAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task<User> CreateUserAsync(RegisterUserDTO registerDto)
        {
            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                PasswordHash = HashPassword(registerDto.Password),
                Role = UserRole.User,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            return await _userRepository.CreateAsync(user);
        }

        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
                return null;

            if (user.PasswordHash != HashPassword(password))
                return null;

            user.LastLoginAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user.Id, user);

            return user;
        }

        public async Task<User> UpdateUserAsync(string userId, UpdateUserDTO updateDto)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user != null)
            {
                user.FirstName = updateDto.FirstName;
                user.LastName = updateDto.LastName;
                user.UpdatedAt = DateTime.UtcNow;

                await _userRepository.UpdateAsync(userId, user);
            }

            return user;
        }

        public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                return false;

            if (user.PasswordHash != HashPassword(currentPassword))
                return false;

            user.PasswordHash = HashPassword(newPassword);
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(userId, user);
            return true;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                return false;

            await _userRepository.DeleteAsync(userId);
            return true;
        }

        public async Task<UserPreferences> UpdateUserPreferencesAsync(string userId, UserPreferencesDTO preferencesDto)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user != null)
            {
                user.Preferences = _mapper.Map<UserPreferences>(preferencesDto);
                user.UpdatedAt = DateTime.UtcNow;

                await _userRepository.UpdateAsync(userId, user);
            }

            return user.Preferences;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
} 