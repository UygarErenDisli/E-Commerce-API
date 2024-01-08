﻿using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using E_Commerce.Application.Abstractions.Storage.Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace E_Commerce.Infrastructure.Services.Storage.Azure
{
	public class AzureStorage : Storage, IAzureStorage
	{
		private readonly BlobServiceClient _blobServiceClient;
		private BlobContainerClient _blobContainerClient;
		public AzureStorage(IConfiguration configuration)
		{
			_blobServiceClient = new(configuration["Storage:Azure"]);
		}
		public async Task DeleteAsync(string containerName, string fileName)
		{
			_blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
			var blobClient = _blobContainerClient.GetBlobClient(fileName);
			await blobClient.DeleteAsync();
		}

		public List<string> GetFiles(string containerName)
		{
			_blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
			return _blobContainerClient.GetBlobs().Select(b => b.Name).ToList();
		}

		public bool HasFile(string containerName, string fileName)
		{
			_blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
			return _blobContainerClient.GetBlobs().Any(b => b.Name == fileName);
		}

		public async Task<List<(string fileName, string path)>> UploadAsync(string containerName, IFormFileCollection files)
		{
			_blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);

			await _blobContainerClient.CreateIfNotExistsAsync();
			await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

			List<(string fileName, string path)> output = new();
			foreach (var file in files)
			{
				string fileNewName = await FileRenameAsync(containerName, file.Name, HasFile);

				BlobClient blobClient = _blobContainerClient.GetBlobClient(fileNewName);
				await blobClient.UploadAsync(file.OpenReadStream());
				output.Add((fileNewName, containerName));
			}
			return output;
		}
	}
}
