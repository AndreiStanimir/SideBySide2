using SideBySideAPI.Models;

namespace SideBySideAPI.Interfaces
{
    public interface ITranslationMemoryService
    {
        Task<TranslationMemory> GetByIdAsync(string id);
        Task<IEnumerable<TranslationMemory>> GetByUserIdAsync(string userId);
        Task<TranslationMemory> CreateAsync(TranslationMemory translationMemory);
        Task<TranslationMemory> UpdateAsync(string id, TranslationMemory translationMemory);
        Task DeleteAsync(string id);
        Task<IEnumerable<TranslationMemory>> SearchAsync(string userId, string sourceLanguage, string text, double minConfidence = 0.7);
    }
} 