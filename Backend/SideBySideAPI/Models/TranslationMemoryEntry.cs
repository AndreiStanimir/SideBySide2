using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SideBySideAPI.Models
{
    public class TranslationMemoryEntry
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("userId")]
        public string UserId { get; set; } = null!;

        [BsonElement("sourceText")]
        public string SourceText { get; set; } = null!;

        [BsonElement("targetText")]
        public string TargetText { get; set; } = null!;

        [BsonElement("sourceLanguage")]
        public string SourceLanguage { get; set; } = null!;

        [BsonElement("targetLanguage")]
        public string TargetLanguage { get; set; } = null!;

        [BsonElement("documentId")]
        public string? DocumentId { get; set; }

        [BsonElement("context")]
        public string? Context { get; set; }

        [BsonElement("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("useCount")]
        public int UseCount { get; set; } = 0;

        [BsonElement("metadata")]
        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();

        [BsonElement("quality")]
        public int Quality { get; set; } = 100; // 0-100 scale
    }
} 