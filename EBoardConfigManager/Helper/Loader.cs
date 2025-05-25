namespace EBoardConfigManager.Helper;

using System.Text.Json;

public static class Loader
{
    public static bool DirExists(string foldername)
    {
        try
        {
            var dir = new DirectoryInfo(foldername);

            return dir.Exists;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public static bool FileExists(string filename)
    {
        try
        {
            var file = new FileInfo(filename);

            return file.Exists;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public static IList<DirectoryInfo> GetDirectories(string directory, SearchOption searchOption = SearchOption.TopDirectoryOnly)
    {
        IList<DirectoryInfo> directoryInfos = [];

        if (string.IsNullOrWhiteSpace(directory))
        {
            return directoryInfos;
        }

        try
        {
            var dirnames = Directory.GetDirectories(directory, "*", searchOption).ToList();

            dirnames.Select(x => x).ToList().ForEach(x =>
            {
                directoryInfos.Add(new DirectoryInfo(x));
            });

            return directoryInfos;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public static IList<FileInfo> GetFiles(string directory, string filter, SearchOption searchOption = SearchOption.TopDirectoryOnly)
    {
        IList<FileInfo> fileInfos = [];

        if (string.IsNullOrWhiteSpace(directory) && string.IsNullOrWhiteSpace(filter))
        {
            return fileInfos;
        }

        try
        {
            var filenames = Directory.GetFiles(directory, filter, searchOption).ToList();

            filenames.Select(x => x).ToList().ForEach(x =>
            {
                fileInfos.Add(new FileInfo(x));
            });

            return fileInfos;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public static async Task<T?> LoadJsonFile<T>(string file)
    {
        if (string.IsNullOrWhiteSpace(file) || !File.Exists(file))
        {
            return (T)Activator.CreateInstance(typeof(T));
        }

        try
        {
            using (FileStream openStream = File.OpenRead(file))
            {
                var instance = JsonSerializer.Deserialize<T>(openStream);
            
                return instance;
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    //public static async Task<T?> LoadXmlFile<T>(string file)
    //{
    //    if (string.IsNullOrWhiteSpace(file) && !File.Exists(file))
    //    {
    //        return (T)Activator.CreateInstance(typeof(T))!;
    //    }

    //    try
    //    {
    //        using FileStream openStream = File.OpenRead(file);

    //        var instance = await JsonSerializer.DeserializeAsync<T>(openStream);

    //        return instance;
    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}

    //        string filter = "*.xml";

    //        List<string> files = Directory.GetFiles(folderPath, filter, SearchOption.TopDirectoryOnly).ToList();

    //        if (files != null && files.Count > 0)
    //        {
    //            foreach (string file in files)
    //            {
    //                if (file.Contains("eboard_") && file.EndsWith($".xml"))
    //                {
    //                    await LoadEboardDataSetFromFile(file, filter);
    //    }
    //}
    //        }
}
