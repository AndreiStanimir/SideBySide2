using SideBySideAPI.Models;
using SideBySideAPI.Models.DTOs;

namespace SideBySideAPI.Interfaces
{
    public interface IDocumentService
    {
        Task<DocumentDTO> GetDocumentByIdAsync(string id);
        Task<IEnumerable<DocumentDTO>> GetDocumentsByUserIdAsync(string userId);
        Task<DocumentDTO> CreateDocumentAsync(CreateDocumentDTO createDocumentDto, string userId);
        Task<DocumentDTO> UpdateDocumentAsync(string id, UpdateDocumentDTO updateDocumentDto);
        Task DeleteDocumentAsync(string id);
        Task<IEnumerable<DocumentSegmentDTO>> GetDocumentSegmentsAsync(string documentId);
        Task<DocumentSegmentDTO> GetDocumentSegmentByIdAsync(string documentId, string segmentId);
        Task<DocumentSegmentDTO> UpdateDocumentSegmentAsync(string documentId, string segmentId, UpdateDocumentSegmentDTO updateDto);
        Task<AnnotationDTO> CreateAnnotationAsync(string documentId, string segmentId, CreateAnnotationDTO createDto, string userId);
        Task<RedactionDTO> CreateRedactionAsync(string documentId, string segmentId, CreateRedactionDTO createDto, string userId);
        Task DeleteAnnotationAsync(string documentId, string segmentId, string annotationId);
        Task DeleteRedactionAsync(string documentId, string segmentId, string redactionId);
    }
} 