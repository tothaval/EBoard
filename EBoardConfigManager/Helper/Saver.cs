using EBoardConfigManager.Enums;
using EBoardConfigManager.Models;
using System.Text.Json;

namespace EBoardConfigManager.Helper;

public static class Saver
{
    public async static Task CreateFolderAsync(string saveFolderPath)
    {
        if (!Directory.Exists(saveFolderPath))
        {
            Directory.CreateDirectory(saveFolderPath);
        }

        await Task.CompletedTask;
    }

    public static Result SaveJsonFile<T>(string filename, T model)
	{
		try
		{
			using (FileStream createStream = File.Create(filename))
			{
				JsonSerializer.Serialize(createStream, model);
			}
        }
		catch (Exception)
		{
			throw;
		}

        return Result.Success;
    }
}
