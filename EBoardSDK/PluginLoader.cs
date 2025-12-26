// <copyright file="PluginLoader.cs" company=".">
// Stephan Kammel
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
namespace EBoardSDK;

using EBoardConfigManager.Helper;
using EBoardSDK.Plugins;
using Serilog;
using System.IO;
using System.Reflection;

public static class PluginLoader
{
    public static async Task<IList<EBoardElementPluginBaseViewModel>> LoadPluginsAsync(string path)
    {
        IList<EBoardElementPluginBaseViewModel> plugins = [];

        var assemblies = Loader.GetFiles(path, "*.dll", SearchOption.TopDirectoryOnly);

        if (assemblies == null || assemblies.Count == 0)
        {
            return plugins;
        }

        assemblies.Select(x => x).ToList().ForEach(dllfile =>
        {
            if (dllfile.Name.Equals("EBoardSDK.dll"))
            {
                return;
            }

            if (!dllfile.Name.StartsWith("EboardElementPlugin")
                && !dllfile.Name.StartsWith("EBoardElementPlugin")
                && !dllfile.Name.StartsWith("EEP_")
                && !dllfile.Name.StartsWith("EEP"))
            {
                return;
            }

            try
            {
                var assembly = Assembly.LoadFrom(dllfile.FullName);
                var types = assembly.GetExportedTypes();

                var baseType = types.Where(dlltype => dlltype.BaseType != null && dlltype.BaseType.Equals(typeof(EBoardElementPluginBaseViewModel))).Any();

                if (!baseType)
                {
                    return;
                }

                var baseviewmodeltype = types.Where(dlltype => dlltype.BaseType!.Equals(typeof(EBoardElementPluginBaseViewModel))).FirstOrDefault();

                if (baseviewmodeltype == null)
                {
                    return;
                }

                try
                {
                    var baseviewmodel = Activator.CreateInstance(baseviewmodeltype) as EBoardElementPluginBaseViewModel;

                    if (baseviewmodel != null)
                    {
                        plugins.Add(baseviewmodel);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);

                    throw;
                }
            }
            catch (Exception)
            {
                // TODO log events of try catch block, specify what dll was skipped
                // prevent not updated external dlls from crashing the application
                // rethrow or pass exception to caller
                return;
            }
        });

        return plugins;
    }
}

// EOF