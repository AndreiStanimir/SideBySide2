using MongoDB.Driver;
using SideBySideAPI.Models;

namespace SideBySideAPI.Interfaces
{
    public interface IMongoDbContext
    {
        IMongoCollection<Document> Documents { get; }
        IMongoCollection<TranslationMemoryEntry> TranslationMemory { get; }
        IMongoCollection<User> Users { get; }
        Task CreateCollectionsIfNotExist();
    }
} 