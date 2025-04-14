using SideBySideAPI.Models;

namespace SideBySideAPI.Interfaces
{
    public interface ITranslationMemoryRepository
    {
        Task<TranslationMemory> GetByIdAsync(string id);
        Task<IEnumerable<TranslationMemory>> GetByUserIdAsync(string userId);
        Task<TranslationMemory> CreateAsync(TranslationMemory translationMemory);
        Task<TranslationMemory> UpdateAsync(string id, TranslationMemory translationMemory);
        Task DeleteAsync(string id);
    }
} 