using AutoMapper;
using MongoDB.Bson;
using SideBySideAPI.Interfaces;
using SideBySideAPI.Models;
using SideBySideAPI.Models.DTOs;

namespace SideBySideAPI.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DocumentService> _logger;

        public DocumentService(
            IDocumentRepository repository,
            IMapper mapper,
            ILogger<DocumentService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<DocumentDTO> GetDocumentByIdAsync(string id)
        {
            var document = await _repository.GetByIdAsync(id);
            return _mapper.Map<DocumentDTO>(document);
        }

        public async Task<IEnumerable<DocumentDTO>> GetDocumentsByUserIdAsync(string userId)
        {
            var documents = await _repository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<DocumentDTO>>(documents);
        }

        public async Task<DocumentDTO> CreateDocumentAsync(CreateDocumentDTO createDocumentDto, string userId)
        {
            var document = new Document
            {
                Name = createDocumentDto.Name,
                UserId = userId,
                FileType = Path.GetExtension(createDocumentDto.File.FileName),
                Status = DocumentStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Save file content
            using (var ms = new MemoryStream())
            {
                await createDocumentDto.File.CopyToAsync(ms);
                document.Content = ms.ToArray();
            }

            var createdDocument = await _repository.CreateAsync(document);
            return _mapper.Map<DocumentDTO>(createdDocument);
        }

        public async Task<DocumentDTO> UpdateDocumentAsync(string id, UpdateDocumentDTO updateDocumentDto)
        {
            var existingDocument = await _repository.GetByIdAsync(id);
            
            existingDocument.Name = updateDocumentDto.Name;
            existingDocument.UpdatedAt = DateTime.UtcNow;

            var updatedDocument = await _repository.UpdateAsync(id, existingDocument);
            return _mapper.Map<DocumentDTO>(updatedDocument);
        }

        public async Task DeleteDocumentAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<DocumentSegmentDTO>> GetDocumentSegmentsAsync(string documentId)
        {
            var document = await _repository.GetByIdAsync(documentId);
            return _mapper.Map<IEnumerable<DocumentSegmentDTO>>(document.Segments);
        }

        public async Task<DocumentSegmentDTO> GetDocumentSegmentByIdAsync(string documentId, string segmentId)
        {
            var document = await _repository.GetByIdAsync(documentId);
            var segment = document.Segments.FirstOrDefault(s => s.Id == segmentId);
            return _mapper.Map<DocumentSegmentDTO>(segment);
        }

        public async Task<DocumentSegmentDTO> UpdateDocumentSegmentAsync(string documentId, string segmentId, UpdateDocumentSegmentDTO updateDto)
        {
            var document = await _repository.GetByIdAsync(documentId);
            var segment = document.Segments.FirstOrDefault(s => s.Id == segmentId);
            
            if (segment != null)
            {
                segment.TargetText = updateDto.TargetText;
                document.UpdatedAt = DateTime.UtcNow;
                await _repository.UpdateAsync(documentId, document);
            }

            return _mapper.Map<DocumentSegmentDTO>(segment);
        }

        public async Task<AnnotationDTO> CreateAnnotationAsync(string documentId, string segmentId, CreateAnnotationDTO createDto, string userId)
        {
            var document = await _repository.GetByIdAsync(documentId);
            var segment = document.Segments.FirstOrDefault(s => s.Id == segmentId);
            
            if (segment != null)
            {
                var annotation = new Annotation
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Text = createDto.Text,
                    StartIndex = createDto.StartIndex,
                    EndIndex = createDto.EndIndex,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId
                };

                segment.Annotations.Add(annotation);
                document.UpdatedAt = DateTime.UtcNow;
                await _repository.UpdateAsync(documentId, document);
                return _mapper.Map<AnnotationDTO>(annotation);
            }

            return null;
        }

        public async Task<RedactionDTO> CreateRedactionAsync(string documentId, string segmentId, CreateRedactionDTO createDto, string userId)
        {
            var document = await _repository.GetByIdAsync(documentId);
            var segment = document.Segments.FirstOrDefault(s => s.Id == segmentId);
            
            if (segment != null)
            {
                var redaction = new Redaction
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    StartIndex = createDto.StartIndex,
                    EndIndex = createDto.EndIndex,
                    Reason = createDto.Reason,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId
                };

                segment.Redactions.Add(redaction);
                document.UpdatedAt = DateTime.UtcNow;
                await _repository.UpdateAsync(documentId, document);
                return _mapper.Map<RedactionDTO>(redaction);
            }

            return null;
        }

        public async Task DeleteAnnotationAsync(string documentId, string segmentId, string annotationId)
        {
            var document = await _repository.GetByIdAsync(documentId);
            var segment = document.Segments.FirstOrDefault(s => s.Id == segmentId);
            
            if (segment != null)
            {
                segment.Annotations.RemoveAll(a => a.Id == annotationId);
                await _repository.UpdateAsync(documentId, document);
            }
        }

        public async Task DeleteRedactionAsync(string documentId, string segmentId, string redactionId)
        {
            var document = await _repository.GetByIdAsync(documentId);
            var segment = document.Segments.FirstOrDefault(s => s.Id == segmentId);
            
            if (segment != null)
            {
                segment.Redactions.RemoveAll(r => r.Id == redactionId);
                await _repository.UpdateAsync(documentId, document);
            }
        }
    }
} 