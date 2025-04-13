using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SideBySideAPI.Models.DTOs
{
    public class DocumentDTO
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string OriginalFileName { get; set; } = null!;
        public string FileType { get; set; } = null!;
        public string SourceLanguage { get; set; } = null!;
        public string TargetLanguage { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long FileSize { get; set; }
        public string ProcessingStatus { get; set; } = null!;
        public int SegmentCount { get; set; }
        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
    }

    public class CreateDocumentDTO
    {
        [Required]
        public IFormFile File { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(10, MinimumLength = 2)]
        public string SourceLanguage { get; set; } = null!;

        [Required]
        [StringLength(10, MinimumLength = 2)]
        public string TargetLanguage { get; set; } = null!;

        [JsonExtensionData]
        public Dictionary<string, object>? Metadata { get; set; }
    }

    public class UpdateDocumentDTO
    {
        [StringLength(100, MinimumLength = 1)]
        public string? Name { get; set; }

        [StringLength(10, MinimumLength = 2)]
        public string? SourceLanguage { get; set; }

        [StringLength(10, MinimumLength = 2)]
        public string? TargetLanguage { get; set; }

        public Dictionary<string, string>? Metadata { get; set; }
    }

    public class DocumentSegmentDTO
    {
        public string Id { get; set; } = null!;
        public string SourceText { get; set; } = null!;
        public string? TargetText { get; set; }
        public int Position { get; set; }
        public List<AnnotationDTO> Annotations { get; set; } = new();
        public List<RedactionDTO> Redactions { get; set; } = new();
    }

    public class UpdateDocumentSegmentDTO
    {
        public string? TargetText { get; set; }
    }

    public class AnnotationDTO
    {
        public string Id { get; set; } = null!;
        public string Text { get; set; } = null!;
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = null!;
    }

    public class CreateAnnotationDTO
    {
        [Required]
        public string Text { get; set; } = null!;

        [Required]
        [Range(0, int.MaxValue)]
        public int StartIndex { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int EndIndex { get; set; }
    }

    public class RedactionDTO
    {
        public string Id { get; set; } = null!;
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public string? Reason { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = null!;
    }

    public class CreateRedactionDTO
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int StartIndex { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int EndIndex { get; set; }

        public string? Reason { get; set; }
    }
} 