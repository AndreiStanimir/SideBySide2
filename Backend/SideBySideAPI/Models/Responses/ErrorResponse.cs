using System.Text.Json.Serialization;

namespace SideBySideAPI.Models.Responses
{
    public class ErrorResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("errors")]
        public List<string>? Errors { get; set; }
    }
} 