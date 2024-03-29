﻿using E_Commerce.Application.Abstractions.Storage.LocalStorage;
using E_Commerce.Application.DTOs.ProductImage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace E_Commerce.Infrastructure.Services.Storage.Local
{
    public class LocalStorage : Storage, ILocalStorage
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LocalStorage(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task DeleteAsync(string path, string fileName) => File.Delete($"{path}\\{fileName}");
        public List<string> GetFiles(string path)
        {
            DirectoryInfo directory = new(path);
            return directory.GetFiles().Select(f => f.Name).ToList();
        }

        public async Task<List<ImageInfoDTO>> UploadAsync(string path, IFormFileCollection files)
        {
            var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            List<ImageInfoDTO> output = [];

            foreach (IFormFile file in files)
            {
                string newFileName = await FileRenameAsync(uploadPath, file.FileName, HasFile);
                await CopyFileAsync(Path.Combine(uploadPath, newFileName), file);
                output.Add(new()
                {
                    FileName = newFileName,
                    Path = $"{uploadPath}/{newFileName}",
                    StorageServiceName = GetType().Name.ToUpper()
                });
            }
            return output;
        }

        public bool HasFile(string path, string fileName) => File.Exists($"{path}\\{fileName}");

        private async Task<bool> CopyFileAsync(string path, IFormFile file)
        {
            try
            {
                using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);

                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();

                return true;
            }
            catch (Exception ex)
            {
                //todo log!
                throw ex;
            }
        }
    }
}
