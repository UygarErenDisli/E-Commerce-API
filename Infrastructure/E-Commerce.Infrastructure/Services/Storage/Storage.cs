using E_Commerce.Infrastructure.Operations;

namespace E_Commerce.Infrastructure.Services.Storage
{
	public class Storage
	{
		protected delegate bool HasFile(string path, string fileName);

		protected async Task<string> FileRenameAsync(string path, string fileName, HasFile hasFileMethod, bool firstTime = true)
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

				if (hasFileMethod(path, newFileName))
				{
					return await FileRenameAsync(path, newFileName, hasFileMethod, false);
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
