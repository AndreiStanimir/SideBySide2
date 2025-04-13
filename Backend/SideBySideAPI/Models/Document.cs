using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace SideBySideAPI.Models
{
    public class Document
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("userId")]
        public string UserId { get; set; } = null!;

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("originalFileName")]
        public string OriginalFileName { get; set; } = null!;

        [BsonElement("fileType")]
        public string FileType { get; set; } = null!;

        [BsonElement("sourceLanguage")]
        public string SourceLanguage { get; set; } = null!;

        [BsonElement("targetLanguage")]
        public string TargetLanguage { get; set; } = null!;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [BsonElement("fileSize")]
        public long FileSize { get; set; }

        [BsonElement("processingStatus")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DocumentProcessingStatus ProcessingStatus { get; set; }

        [BsonElement("segments")]
        public List<DocumentSegment> Segments { get; set; } = new List<DocumentSegment>();

        [BsonElement("metadata")]
        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();

        [BsonElement("fileId")]
        public string? FileId { get; set; }
    }

    public enum DocumentProcessingStatus
    {
        Pending,
        Processing,
        OCRInProgress,
        Completed,
        Failed
    }

    public class DocumentSegment
    {
        [BsonElement("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [BsonElement("sourceText")]
        public string SourceText { get; set; } = null!;

        [BsonElement("targetText")]
        public string? TargetText { get; set; }

        [BsonElement("position")]
        public int Position { get; set; }

        [BsonElement("annotations")]
        public List<Annotation> Annotations { get; set; } = new List<Annotation>();

        [BsonElement("redactions")]
        public List<Redaction> Redactions { get; set; } = new List<Redaction>();
    }

    public class Annotation
    {
        [BsonElement("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [BsonElement("text")]
        public string Text { get; set; } = null!;

        [BsonElement("startIndex")]
        public int StartIndex { get; set; }

        [BsonElement("endIndex")]
        public int EndIndex { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("createdBy")]
        public string CreatedBy { get; set; } = null!;
    }

    public class Redaction
    {
        [BsonElement("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [BsonElement("startIndex")]
        public int StartIndex { get; set; }

        [BsonElement("endIndex")]
        public int EndIndex { get; set; }

        [BsonElement("reason")]
        public string? Reason { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("createdBy")]
        public string CreatedBy { get; set; } = null!;
    }
} 