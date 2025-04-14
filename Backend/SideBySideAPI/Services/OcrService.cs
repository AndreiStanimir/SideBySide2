using SideBySideAPI.Interfaces;

namespace SideBySideAPI.Services
{
    public class OcrService : IOcrService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly ILogger<OcrService> _logger;

        public OcrService(
            IDocumentRepository documentRepository,
            ILogger<OcrService> logger)
        {
            _documentRepository = documentRepository;
            _logger = logger;
        }

        public async Task ProcessDocumentAsync(string documentId)
        {
            try
            {
                var document = await _documentRepository.GetByIdAsync(documentId);
                if (document == null)
                {
                    _logger.LogWarning("Document not found for OCR processing: {DocumentId}", documentId);
                    return;
                }

                // TODO: Implement actual OCR processing
                // For now, just update the status
                document.Status = Models.DocumentStatus.Processed;
                document.UpdatedAt = DateTime.UtcNow;

                await _documentRepository.UpdateAsync(documentId, document);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing document with OCR: {DocumentId}", documentId);
                throw;
            }
        }

        public async Task<string> ExtractTextFromImageAsync(Stream imageStream)
        {
            // TODO: Implement actual OCR text extraction
            await Task.Delay(100); // Simulate processing
            return "Sample extracted text";
        }

        public async Task<bool> IsOcrServiceAvailableAsync()
        {
            // TODO: Implement actual OCR service availability check
            await Task.Delay(100); // Simulate check
            return true;
        }
    }
} 