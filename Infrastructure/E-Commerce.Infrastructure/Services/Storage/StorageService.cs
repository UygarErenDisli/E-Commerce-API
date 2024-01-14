using E_Commerce.Application.Abstractions.Storage;
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

		public Task<List<(string fileName, string path)>> UploadAsync(string path, IFormFileCollection files) => _storage.UploadAsync(path, files);
	}
}
