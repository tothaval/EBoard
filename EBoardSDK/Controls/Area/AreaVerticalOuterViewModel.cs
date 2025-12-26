// <copyright file="AreaVerticalOuterViewModel.cs" company=".">
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
using System.Collections.ObjectModel;
using System.Linq;

/// <summary>
/// TODO
/// sinnvoll refaktorn und Methoden auslagern.
/// 
/// // make CreateNewArea a task?
/// </summary>
public partial class AreaVerticalOuterViewModel<T> : ObservableObject
        where T : EBoardElementPluginBaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<AreaHorizontalInnerViewModel<T>> horizontalInnerViewModels;

    [ObservableProperty]
    private int verticalCount = 0;

    [ObservableProperty]
    private int horizontalCount = 0;

    private ElementViewModel elementViewModel;

    public AreaVerticalOuterViewModel()
    {
    }

    public AreaVerticalOuterViewModel(ElementViewModel elementViewModel)
    {
        this.HorizontalInnerViewModels = new ObservableCollection<AreaHorizontalInnerViewModel<T>>();

        this.elementViewModel = elementViewModel;
    }

    public void CreateMatrix(int x, int y)
    {
        this.HorizontalCount = x;
        this.VerticalCount = y;

        this.CreateNewArea();
    }

    public void InsertViewModelList(List<T> viewModels, int verticalIndex)
    {
        if (verticalIndex < this.HorizontalInnerViewModels.Count)
        {
            this.HorizontalCount = viewModels.Count;

            this.HorizontalInnerViewModels[verticalIndex].InsertViewModelList(viewModels);
        }
    }

    public void AddLine()
    {
        this.VerticalCount++;

        this.CreateNewArea();

        this.OnPropertyChanged(nameof(this.HorizontalInnerViewModels));
    }

    public void RemoveLine()
    {
        if (this.VerticalCount > 0)
        {
            this.VerticalCount--;
        }

        if (this.HorizontalInnerViewModels.Count == 0)
        {
            return;
        }

        var vm = this.HorizontalInnerViewModels.LastOrDefault();

        if (vm != null)
        {
            this.HorizontalInnerViewModels.Remove(vm);

            this.OnPropertyChanged(nameof(this.HorizontalInnerViewModels));
        }
    }

    public void AddRow()
    {
        this.HorizontalCount++;

        this.CreateNewArea();

        this.OnPropertyChanged(nameof(this.HorizontalInnerViewModels));
    }

    public void RemoveRow()
    {
        if (this.HorizontalCount > 0)
        {
            this.HorizontalCount--;
        }

        if (this.HorizontalInnerViewModels.Count == 0)
        {
            return;
        }

        foreach (var item in this.HorizontalInnerViewModels)
        {
            item.RemoveElement();
        }
    }

    public void SetElementViewModel(ElementViewModel elementViewModel)
    {
        this.elementViewModel = elementViewModel;
    }

    private void AddAreaHorizontalInnerViewModel()
    {
        if (this.elementViewModel != null)
        {
            var areahorizontal = new AreaHorizontalInnerViewModel<T>(this.elementViewModel);

            this.HorizontalInnerViewModels.Add(areahorizontal);
        }
    }

    private void CreateNewArea()
    {
        if (this.VerticalCount == 0)
        {
            this.VerticalCount++;
        }

        if (this.HorizontalCount == 0)
        {
            this.HorizontalCount++;
        }

        var areaVMcount = this.HorizontalInnerViewModels.Count;

        for (int i = 0; i < this.VerticalCount; i++)
        {
            if (i >= areaVMcount)
            {
                this.AddAreaHorizontalInnerViewModel();
            }
        }

        foreach (var item in this.HorizontalInnerViewModels)
        {
            var elementsCount = item.Elements.Count;

            for (int i = 0; i < this.HorizontalCount; i++)
            {
                if (i >= elementsCount)
                {
                    item.AddElement();
                }
            }
        }

        this.OnPropertyChanged(nameof(this.HorizontalInnerViewModels));
    }
}

// EOF