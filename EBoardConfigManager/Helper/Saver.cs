// <copyright file="Saver.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using EBoardConfigManager.Enums;
using Serilog;
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
                JsonSerializer.Serialize(createStream, model, ConfigOptions.JsonSerializerOptions);
            }
        }
        catch (ArgumentException ex)
        {
            var s = $"filename: {filename}, type {typeof(T)}, model {model}";

            Log.Error(ex, s);

            File.Delete(filename);
        }
        catch (Exception)
        {
            throw;
        }

        return Result.Success;
    }
}
