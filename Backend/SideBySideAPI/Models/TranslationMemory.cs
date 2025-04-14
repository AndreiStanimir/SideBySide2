using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SideBySideAPI.Models
{
    public class TranslationMemory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("userId")]
        public string UserId { get; set; } = string.Empty;

        [BsonElement("sourceLanguage")]
        public string SourceLanguage { get; set; } = string.Empty;

        [BsonElement("targetLanguage")]
        public string TargetLanguage { get; set; } = string.Empty;

        [BsonElement("sourceText")]
        public string SourceText { get; set; } = string.Empty;

        [BsonElement("targetText")]
        public string TargetText { get; set; } = string.Empty;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("documentId")]
        public string? DocumentId { get; set; }

        [BsonElement("confidence")]
        public double Confidence { get; set; }

        [BsonElement("isVerified")]
        public bool IsVerified { get; set; }
    }
} 