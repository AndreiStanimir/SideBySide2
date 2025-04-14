using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SideBySideAPI.Models;

namespace SideBySideAPI.Models.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; }
        public UserPreferencesDTO Preferences { get; set; } = new UserPreferencesDTO();
    }

    public class RegisterUserDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;
    }

    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class UpdateUserDTO
    {
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;
    }

    public class ChangePasswordDTO
    {
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }

    public class TokenResponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public UserDTO User { get; set; } = null!;
    }

    public class UserPreferencesDTO
    {
        public string DefaultSourceLanguage { get; set; } = "en";
        public string DefaultTargetLanguage { get; set; } = "es";
        public string Theme { get; set; } = "light";
        public string UILanguage { get; set; } = "en";
        public bool UseAutomaticTranslation { get; set; } = true;
        public bool ShowLineNumbers { get; set; } = true;
        public int FontSizeSource { get; set; } = 14;
        public int FontSizeTarget { get; set; } = 14;
    }
} 