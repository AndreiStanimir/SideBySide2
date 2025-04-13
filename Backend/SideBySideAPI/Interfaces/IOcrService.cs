namespace SideBySideAPI.Interfaces
{
    public interface IOcrService
    {
        Task ProcessDocumentAsync(string documentId);
        Task<string> ExtractTextFromImageAsync(Stream imageStream);
        Task<bool> IsOcrServiceAvailableAsync();
    }
} 