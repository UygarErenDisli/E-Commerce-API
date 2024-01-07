using E_Commerce.Application.Services;
using E_Commerce.Infrastructure.Operations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace E_Commerce.Infrastructure.Services
{
	public class FileService : IFileService
	{
		private readonly IWebHostEnvironment _webHostEnvironment;

		public FileService(IWebHostEnvironment webHostEnvironment)
		{
			_webHostEnvironment = webHostEnvironment;
		}

		public async Task<bool> CopyFileAsync(string path, IFormFile file)
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

		public async Task<List<(string fileName, string path)>> UploadAsync(string path, IFormFileCollection files)
		{
			var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);

			if (!Directory.Exists(uploadPath))
			{
				Directory.CreateDirectory(uploadPath);
			}
			List<(string fileName, string path)> output = new();
			List<bool> results = new();
			foreach (IFormFile file in files)
			{
				string newFileName = await FileRenameAsync(uploadPath, file.FileName);
				bool result = await CopyFileAsync(Path.Combine(uploadPath, newFileName), file);
				output.Add((fileName: newFileName, path: $"{uploadPath}\\{newFileName}"));
				results.Add(result);
			}
			if (results.TrueForAll(r => r.Equals(true)))
			{
				return output;

			}
			//todo add a custom exception for else statement
			return null;
		}

		private async Task<string> FileRenameAsync(string uploadPath, string fileName, bool firstTime = true)
		{
			var newFileName = await Task.Run<string>(async () =>
			{
				string extention = Path.GetExtension(fileName);
				string newFileName = string.Empty;
				if (firstTime)
				{
					string oldName = Path.GetFileNameWithoutExtension(fileName);
					newFileName = $"{NameOperation.CharacterRegulatory(oldName)}{extention}";
				}
				else
				{
					newFileName = fileName;
					int dashIndex = newFileName.IndexOf("-");
					if (dashIndex == -1)
					{
						newFileName = $"{Path.GetFileNameWithoutExtension(fileName)}-2{extention}";
					}
					else
					{
						int lastDashIndex = 0;
						while (true)
						{
							lastDashIndex = dashIndex;
							dashIndex = newFileName.IndexOf("-", dashIndex + 1);
							if (dashIndex == -1)
							{
								dashIndex = lastDashIndex;
								break;
							}
						}
						int dotIndex = newFileName.IndexOf(".");
						string _fileNo = newFileName.Substring(dashIndex + 1, dotIndex - dashIndex - 1);
						if (int.TryParse(_fileNo, out int fileNo))
						{
							fileNo++;
							newFileName = newFileName.Remove(dashIndex + 1, dotIndex - dashIndex - 1).Insert(dashIndex + 1, fileNo.ToString());
						}
						else
						{
							newFileName = $"{Path.GetFileNameWithoutExtension(fileName)}-2{extention}";
						}
					}
				}

				if (File.Exists($"{uploadPath}\\{newFileName}"))
				{
					return await FileRenameAsync(uploadPath, newFileName, false);
				}
				else
				{
					return newFileName;
				}
			});
			return newFileName;
		}
	}
}
