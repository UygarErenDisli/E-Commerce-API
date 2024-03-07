using E_Commerce.Application.DTOs.ProductImage;
using Microsoft.AspNetCore.Http;

namespace E_Commerce.Application.Abstractions.Storage
{
    public interface IStorage
    {
        Task<List<ImageInfoDTO>> UploadAsync(string path, IFormFileCollection files);
        Task DeleteAsync(string path, string fileName);
        List<string> GetFiles(string path);
        bool HasFile(string path, string fileName);
    }
}
