using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SideBySideAPI.Models.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Role { get; set; } = null!;
        public UserPreferencesDTO? Preferences { get; set; }
    }

    public class RegisterUserDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = null!;

        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }
    }

    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }

    public class TokenResponseDTO
    {
        public string Token { get; set; } = null!;
        public UserDTO User { get; set; } = null!;
    }

    public class UpdateUserDTO
    {
        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        [StringLength(100, MinimumLength = 6)]
        public string? Password { get; set; }

        public UserPreferencesDTO? Preferences { get; set; }
    }

    public class UserPreferencesDTO
    {
        [StringLength(10)]
        public string? DefaultSourceLanguage { get; set; }

        [StringLength(10)]
        public string? DefaultTargetLanguage { get; set; }

        [StringLength(20)]
        public string? Theme { get; set; }

        [StringLength(10)]
        public string? UILanguage { get; set; }

        public bool? UseAutomaticTranslation { get; set; }

        public bool? ShowLineNumbers { get; set; }

        [Range(8, 36)]
        public int? FontSizeSource { get; set; }

        [Range(8, 36)]
        public int? FontSizeTarget { get; set; }
    }
} 