using MongoDB.Driver;
using SideBySideAPI.Models;

namespace SideBySideAPI.Interfaces
{
    public interface IMongoDbContext
    {
        IMongoCollection<Document> Documents { get; }
        IMongoCollection<TranslationMemory> TranslationMemory { get; }
        IMongoCollection<User> Users { get; }
        Task CreateCollectionsIfNotExist();
        IMongoCollection<T> GetCollection<T>(string name);
    }
} 