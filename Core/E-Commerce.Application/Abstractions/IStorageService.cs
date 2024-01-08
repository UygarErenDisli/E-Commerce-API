using E_Commerce.Application.Abstractions.Storage;

namespace E_Commerce.Application.Abstractions
{
	public interface IStorageService : IStorage
	{
		public string StorageName { get; }
	}

}
