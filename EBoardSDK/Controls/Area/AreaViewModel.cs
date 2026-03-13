// <copyright file="AreaViewModel.cs" company=".">
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
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Interfaces;
using EBoardSDK.Plugins;
using EBoardSDK.ViewModels;
using System.Security.Cryptography;

public partial class AreaViewModel<T> : ObservableObject
        where T : EBoardElementPluginBaseViewModel
{
    private ElementViewModel elementViewModel;

    [ObservableProperty]
    private bool isMatrixChangeable = true;

    /// <summary>
    /// Initializes a new instance of the <see cref="AreaViewModel{T}"/> class.
    /// </summary>
    public AreaViewModel()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AreaViewModel{T}"/> class.
    /// </summary>
    /// <param name="elementViewModel"></param>
    public AreaViewModel(ElementViewModel elementViewModel)
    {
        this.elementViewModel = elementViewModel;

        this.AreaVerticalOuterViewModel = new(elementViewModel);

        this.IsMatrixChangeable = true;
        this.OnPropertyChanged(nameof(this.AreaVerticalOuterViewModel));
        this.OnPropertyChanged(nameof(this.ElementViewModel));
    }

    public ElementViewModel ElementViewModel => this.elementViewModel;

    public AreaVerticalOuterViewModel<T> AreaVerticalOuterViewModel { get; }

    public void SetElementViewModel(ElementViewModel elementViewModel)
    {
        this.elementViewModel = elementViewModel;

        this.AreaVerticalOuterViewModel?.SetElementViewModel(elementViewModel);

        this.OnPropertyChanged(nameof(this.ElementViewModel));
        this.OnPropertyChanged(nameof(this.AreaVerticalOuterViewModel));
    }

    public void AddHorizontal()
    {
        this.AreaVerticalOuterViewModel?.AddLine();
        this.OnPropertyChanged(nameof(this.AreaVerticalOuterViewModel));
    }

    public void CreateMatrix(int horizontalCount, int verticalCount)
    {
        this.AreaVerticalOuterViewModel?.CreateMatrix(horizontalCount, verticalCount);

        this.IsMatrixChangeable = false;
        this.OnPropertyChanged(nameof(this.AreaVerticalOuterViewModel));
    }

    public List<List<T>> GetAllViewModelsList()
    {
        List<List<T>> listlist = new List<List<T>>();

        if (this.AreaVerticalOuterViewModel == null)
        {
            return listlist;
        }

        foreach (var item in this.AreaVerticalOuterViewModel.HorizontalInnerViewModels)
        {
            var list = new List<T>();

            foreach (var horizontalelement in item.Elements)
            {
                list.Add(horizontalelement);
            }

            listlist.Add(list);
        }

        return listlist;
    }

    public void InsertViewModelList(List<T> viewModels, int verticalIndex)
    {
        this.AreaVerticalOuterViewModel?.InsertViewModelList(viewModels, verticalIndex);
        this.OnPropertyChanged(nameof(this.AreaVerticalOuterViewModel));
    }

    [RelayCommand]
    public void AddLine()
    {
        this.AreaVerticalOuterViewModel?.AddLine();
        this.OnPropertyChanged(nameof(this.AreaVerticalOuterViewModel));
    }

    [RelayCommand]
    public void RemoveLine()
    {
        this.AreaVerticalOuterViewModel?.RemoveLine();
        this.OnPropertyChanged(nameof(this.AreaVerticalOuterViewModel));
    }

    [RelayCommand]
    public void AddRow()
    {
        this.AreaVerticalOuterViewModel?.AddRow();
        this.OnPropertyChanged(nameof(this.AreaVerticalOuterViewModel));
    }

    [RelayCommand]
    public void RemoveRow()
    {
        this.AreaVerticalOuterViewModel?.RemoveRow();
        this.OnPropertyChanged(nameof(this.AreaVerticalOuterViewModel));
    }

    [RelayCommand]
    private void ResetArea()
    {
        this.AreaVerticalOuterViewModel?.ResetArea();
        this.OnPropertyChanged(nameof(this.AreaVerticalOuterViewModel));
    }
}

// EOF