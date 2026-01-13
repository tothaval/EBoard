// <copyright file="EBoardElementPluginBaseViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins;

using CommunityToolkit.Mvvm.ComponentModel;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.Utilities;
using EBoardSDK.ViewModels;
using Serilog;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

public abstract partial class EBoardElementPluginBaseViewModel : ObservableObject, IPlugin
{
    private EBoardViewModel eBoardViewModel;

    protected ElementViewModel elementViewModel;

    public event Action? PropertyChangedEvent;

    public abstract Assembly? ElementPluginAssembly { get; }

    public abstract Type? ElementPluginModel { get; }

    public abstract Type ElementPluginView { get; }

    public abstract Type ElementPluginViewModel { get; }

    public ElementScreenIntegrationConstraints? ElementScreenIntegrationConstraints { get; set; } = new ElementScreenIntegrationConstraints(ElementInstantiationPolicy.ValueNotSet);

    public abstract bool NoDefaultBorders { get; }

    public abstract UserControl Plugin { get; }

    public abstract PluginCategories PluginCategory { get; }

    public abstract ImageBrush PluginLogo { get; set; }

    public abstract string PluginName { get; set; }

    public abstract string PluginHeader { get; set; }

    public abstract ResourceDictionary ResourceDictionary { get; }

    public EBoardViewModel EBoardViewModel => this.eBoardViewModel;

    public ElementViewModel ElementViewModel => this.elementViewModel;

    public IFluidUIContext FluidUI => throw new NotImplementedException();

    public virtual void Dispose()
    {
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    public async Task<bool> Initialize()
    {
        try
        {
            var path = await new SDKDataManager().GetInstalledPluginsDirectory();

            if (string.IsNullOrWhiteSpace(this.PluginName) || string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            var resourcestring = Path.Combine(path, $"{this.PluginName}_Logo.png");

            var imagefile = new FileInfo(resourcestring);

            if (!imagefile.Exists)
            {
                return false;
            }

            var imagesource = new BitmapImage(new Uri(imagefile.FullName));

            this.PluginLogo = new ImageBrush(imagesource);

            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }

        return false;
    }

    public abstract Task<EBoardFeedbackMessage> Load(string path);

    public abstract Task<EBoardFeedbackMessage> Save(string path);

    /// <summary>
    /// use this in IPlugin deriving viewmodel to reload certain stuff, that couldn't load.
    /// i am not that familiar with Activator.CreateInstance overloads yet to know what i am doing,
    /// that is why i stick to empty constructors until that changes.
    ///
    /// when only an empty constructor is used, it can be that due to instantiaton time or missing object
    /// availability, that at the time the constructor is initiated certain data or viewmodels can not be
    /// found or accessed, or due to the plugin architecture.
    ///
    /// if you override this and make it so that before the call all data is given to the object,
    /// you can circumvent that issue.
    ///
    /// so far this is needed only in edge cases.
    /// </summary>
    public virtual void RefreshInitialization()
    {
        this.ViewWasSet();
    }

    public void SetEBoardAndElementViewModel(EBoardViewModel eBoardViewModel, ElementViewModel elementViewModel)
    {
        this.eBoardViewModel = eBoardViewModel;
        this.elementViewModel = elementViewModel;

        this.OnPropertyChanged(nameof(this.EBoardViewModel));
        this.OnPropertyChanged(nameof(this.ElementViewModel));
        this.OnPropertyChanged(nameof(this.Plugin));
    }

    public virtual void SetInitialValues()
    {
    }

    public virtual void ViewWasSet()
    {
    }
}

// EOF