using MongoDB.Driver;
using SideBySideAPI.Interfaces;
using SideBySideAPI.Models;

namespace SideBySideAPI.Services
{
    public class TranslationMemoryService : ITranslationMemoryService
    {
        private readonly ITranslationMemoryRepository _repository;
        private readonly ILogger<TranslationMemoryService> _logger;

        public TranslationMemoryService(
            ITranslationMemoryRepository repository,
            ILogger<TranslationMemoryService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<TranslationMemory> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<TranslationMemory>> GetByUserIdAsync(string userId)
        {
            return await _repository.GetByUserIdAsync(userId);
        }

        public async Task<TranslationMemory> CreateAsync(TranslationMemory translationMemory)
        {
            translationMemory.CreatedAt = DateTime.UtcNow;
            translationMemory.UpdatedAt = DateTime.UtcNow;
            return await _repository.CreateAsync(translationMemory);
        }

        public async Task<TranslationMemory> UpdateAsync(string id, TranslationMemory translationMemory)
        {
            translationMemory.UpdatedAt = DateTime.UtcNow;
            return await _repository.UpdateAsync(id, translationMemory);
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<TranslationMemory>> SearchAsync(string userId, string sourceLanguage, string text, double minConfidence = 0.7)
        {
            var allEntries = await _repository.GetByUserIdAsync(userId);
            return allEntries
                .Where(tm => 
                    tm.SourceLanguage == sourceLanguage && 
                    tm.Confidence >= minConfidence &&
                    (tm.SourceText.Contains(text, StringComparison.OrdinalIgnoreCase) ||
                     text.Contains(tm.SourceText, StringComparison.OrdinalIgnoreCase)))
                .OrderByDescending(tm => tm.Confidence);
        }
    }
} 