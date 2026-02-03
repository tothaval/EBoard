// <copyright file="AreaHorizontalInnerViewModel.cs" company=".">
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
namespace EBoardSDK.Controls.Area;

using CommunityToolkit.Mvvm.ComponentModel;
using EBoardSDK.Plugins;
using EBoardSDK.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;

public partial class AreaHorizontalInnerViewModel<T> : ObservableObject
    where T : PluginBaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<T> elements = new ();

    [ObservableProperty]
    private T? element;

    private ElementViewModel? elementViewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="AreaHorizontalInnerViewModel{T}"/> class.
    /// </summary>
    public AreaHorizontalInnerViewModel()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AreaHorizontalInnerViewModel{T}"/> class.
    /// </summary>
    /// <param name="elementViewModel"></param>
    public AreaHorizontalInnerViewModel(ElementViewModel elementViewModel)
    {
        this.elementViewModel = elementViewModel;
    }

    public void InsertViewModelList(List<T> viewModels)
    {
        this.Elements.Clear();

        foreach (var model in viewModels)
        {
            this.AddElement(model);
        }

        this.OnPropertyChanged(nameof(this.Elements));
        this.OnPropertyChanged(nameof(this.Element));
    }

    public void AddElement(T plugin)
    {
        plugin.RefreshInitialization();
        this.Elements?.Add(plugin);

        this.OnPropertyChanged(nameof(this.Elements));
    }

    public void CreateElement()
    {
        var instance = Activator.CreateInstance<T>();

        if (instance != null)
        {
            if (this.elementViewModel != null)
            {
                instance.SetElementViewModel(this.elementViewModel);
                instance.RefreshInitialization();
            }

            this.Elements?.Add(instance);
        }

        this.OnPropertyChanged(nameof(this.Elements));
    }

    public void RemoveElement()
    {
        var last = this.Elements.LastOrDefault();

        if (last != null)
        {
            this.Elements?.Remove(last);
            this.OnPropertyChanged(nameof(this.Elements));
        }
    }
}

// EOF