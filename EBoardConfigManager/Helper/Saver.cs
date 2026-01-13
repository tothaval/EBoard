// <copyright file="Saver.cs" company="PlaceholderCompany">
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

using EBoardConfigManager.Enums;
using Serilog;
using System.Text.Json;

public static class Saver
{
    public static bool CleanFolderAsync(string folder)
    {
        string filter = "*.*";

        List<string> files = Directory.GetFiles(folder, filter, SearchOption.AllDirectories).ToList();

        if (files.Count > 0)
        {
            foreach (string file in files)
            {
                File.Delete(file);
            }
        }

        List<string> folders = Directory.GetDirectories(folder).ToList();

        if (folders.Count > 0)
        {
            foreach (string f in folders)
            {
                try
                {
                    Directory.Delete(f, true);
                }
                catch (Exception)
                {
                    // diese exception mal handlen oder im try block prüfen,
                    // ob die datei frei oder in verwendung ist, ggf. ein paar
                    // mal wiederholen bis zum abbruch

                    // mitunter ist die shapedata.xml noch von einem anderen prozess
                    // blockiert, aktuell keine ahnung weswegen, low prio
                }
            }
        }

        return true;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="saveFolderPath"></param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    public static async Task CreateFolderAsync(string saveFolderPath)
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
            if (string.IsNullOrWhiteSpace(filename))
            {
                return Result.Error;
            }

            using (FileStream createStream = File.Create(filename))
            {
                JsonSerializer.Serialize(createStream, model, ConfigOptions.JsonSerializerOptions);
            }
        }
        catch (ArgumentNullException anex)
        {
            var s = $"filename: {filename}, type {typeof(T)}, model {model}";

            Log.Error(anex, s);

            File.Delete(filename);
            return Result.Error;
        }
        catch (NotSupportedException ex)
        {
            var s = $"filename: {filename}, type {typeof(T)}, model {model}";

            Log.Error(ex, s);

            File.Delete(filename);
            return Result.Error;
        }
        catch (Exception exc)
        {
            var s = $"filename: {filename}, type {typeof(T)}, model {model}";

            Log.Error(exc, s);

            File.Delete(filename);
            return Result.Error;
        }

        return Result.Success;
    }
}

// EOF