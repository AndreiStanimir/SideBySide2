using SideBySideAPI.Models;

namespace SideBySideAPI.Interfaces
{
    public interface IDocumentRepository
    {
        Task<Document> GetByIdAsync(string id);
        Task<IEnumerable<Document>> GetByUserIdAsync(string userId);
        Task<Document> CreateAsync(Document document);
        Task<Document> UpdateAsync(string id, Document document);
        Task DeleteAsync(string id);
    }
} 