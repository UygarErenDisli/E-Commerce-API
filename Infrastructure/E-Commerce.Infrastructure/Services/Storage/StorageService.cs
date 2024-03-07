using E_Commerce.Application.Abstractions.Storage;
using E_Commerce.Application.DTOs.ProductImage;
using Microsoft.AspNetCore.Http;

namespace E_Commerce.Infrastructure.Services.Storage
{
    public class StorageService : IStorageService
    {
        private readonly IStorage _storage;

        public StorageService(IStorage storage)
        {
            _storage = storage;
        }
        public string StorageName => _storage.GetType().Name;

        public async Task DeleteAsync(string path, string fileName) => await _storage.DeleteAsync(path, fileName);

        public List<string> GetFiles(string path) => _storage.GetFiles(path);

        public bool HasFile(string path, string fileName) => _storage.HasFile(path, fileName);

        public Task<List<ImageInfoDTO>> UploadAsync(string path, IFormFileCollection files) => _storage.UploadAsync(path, files);
    }
}
