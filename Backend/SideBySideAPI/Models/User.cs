using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace SideBySideAPI.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("username")]
        public string Username { get; set; } = null!;

        [BsonElement("email")]
        public string Email { get; set; } = null!;

        [BsonElement("passwordHash")]
        [JsonIgnore]
        public string PasswordHash { get; set; } = null!;

        [BsonElement("firstName")]
        public string? FirstName { get; set; }

        [BsonElement("lastName")]
        public string? LastName { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("lastLoginAt")]
        public DateTime? LastLoginAt { get; set; }

        [BsonElement("role")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserRole Role { get; set; } = UserRole.User;

        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;

        [BsonElement("preferences")]
        public UserPreferences Preferences { get; set; } = new UserPreferences();
    }

    public enum UserRole
    {
        User,
        Admin
    }

    public class UserPreferences
    {
        [BsonElement("defaultSourceLanguage")]
        public string DefaultSourceLanguage { get; set; } = "en";

        [BsonElement("defaultTargetLanguage")]
        public string DefaultTargetLanguage { get; set; } = "es";

        [BsonElement("theme")]
        public string Theme { get; set; } = "light";

        [BsonElement("uiLanguage")]
        public string UILanguage { get; set; } = "en";

        [BsonElement("useAutomaticTranslation")]
        public bool UseAutomaticTranslation { get; set; } = true;

        [BsonElement("showLineNumbers")]
        public bool ShowLineNumbers { get; set; } = true;

        [BsonElement("fontSizeSource")]
        public int FontSizeSource { get; set; } = 14;

        [BsonElement("fontSizeTarget")]
        public int FontSizeTarget { get; set; } = 14;
    }
} 