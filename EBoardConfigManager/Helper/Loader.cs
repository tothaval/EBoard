// <copyright file="Loader.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
/// license
///
/// <b>ad-hoc license terms eboard prototype</b><br>
/// <br>
/// <br>
/// contact: kammel@posteo.de
/// <br>
/// <p>
/// until a license has been chosen, you may 
/// use the software or parts of it under the following conditions:<br><br>
/// 1.)
/// If you want to distribute or use the source code or a derived binary
/// of the EBoard project for commercial purposes, you need to contact
/// the project team for authorization and payment details.
/// You may use the source or a derived binary for non commercial 
/// purposes free of charge. In order to do so, copy this adhoc terms
/// and a link to the repository to any source code file that uses code
/// derived from this project and to the folder that holds the compiled source code.
///
/// 2.)
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
/// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
/// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
/// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
/// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
/// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
/// OTHER DEALINGS IN THE SOFTWARE.
/// </p>
namespace EBoardConfigManager.Helper;

using Serilog;
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

        if (string.IsNullOrWhiteSpace(directory) || !DirExists(directory))
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

        if (!Directory.Exists(directory))
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
                var instance = JsonSerializer.Deserialize<T>(openStream, ConfigOptions.JsonSerializerOptions);

                openStream.Dispose(); // hopefully the using block collapses when catching an exception.
                return instance;
            }
        }
        catch (ArgumentException ex)
        {
            var s = $"filename: {file}, type {typeof(T)}";

            Log.Error($"empty or damaged config file {s}");
            Log.Error(ex, s);

            //throw new ArgumentException(string.Join("__ ", s, ex.Message));
        }
        catch (System.Text.Json.JsonException jsonEx)
        {
            var s = $"filename: {file}, type {typeof(T)}";

            Log.Error($"empty or damaged config file {s}");
            Log.Error(jsonEx, s);

            //throw new System.Text.Json.JsonException(string.Join("__ ", s, jsonEx.Message));
        }
        catch (Exception e)
        {
            var s = $"filename: {file}, type {typeof(T)}";

            Log.Error($"empty or damaged config file {s}");
            Log.Error(e, s);
        }

        return (T)Activator.CreateInstance(typeof(T));
    }
}

// EOF