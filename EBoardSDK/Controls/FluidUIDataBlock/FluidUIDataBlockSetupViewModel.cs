// <copyright file="FluidUIDataBlockSetupViewModel.cs" company=".">
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
namespace EBoardSDK.Controls.FluidUIDataBlock;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Controls.FluidUIDataBlock.FluidUIIndexText;
using EBoardSDK.Interfaces.FluidUIDataBlock;
using EBoardSDK.ViewModels;
using System;
using System.Collections.ObjectModel;

public partial class FluidUIDataBlockSetupViewModel : ObservableObject, IFluidUIDataBlockSetup
{
    private EboardFluidUIBaseViewModel viewModel;

    [ObservableProperty]
    private FluidUIIndexTextViewModel fluidUIIndexText;

    [ObservableProperty]
    private string text = string.Empty;

    [ObservableProperty]
    private int indexCount = 0;

    [ObservableProperty]
    private ObservableCollection<FluidUIIndexTextViewModel> fluidUIIndexTexts = new();

    public FluidUIDataBlockSetupViewModel(EboardFluidUIBaseViewModel eboardFluidUIBaseViewModel)
    {
        this.viewModel = eboardFluidUIBaseViewModel;

        foreach (var item in this.ViewModel.FluidUI.DataBlock.IndexTextList)
        {
            this.FluidUIIndexTexts.Add(new FluidUIIndexTextViewModel(this.ViewModel, item));
        }

        this.FluidUIIndexText = this.FluidUIIndexTexts.FirstOrDefault()!;
    }

    public event Action PropertyChangedEvent;

    public EboardFluidUIBaseViewModel ViewModel => this.viewModel;


    public void Dispose()
    {
    }

    public void SetInitialValues()
    {
    }

    //partial void OnTextChanged(string value)
    //{
    //    if (this.FluidUIIndexText != null)
    //    {
    //        var element = this.ViewModel.FluidUI.DataBlock.IndexTextList.Where(x => x.Index.Equals(this.FluidUIIndexText.Index)).FirstOrDefault();

    //        if (element != null && !element.Text.Equals(value))
    //        {
    //            element.Text = value;
    //        }

    //        this.OnPropertyChanged(nameof(this.FluidUIIndexTexts));
    //    }
    //}

    [RelayCommand]
    private void AddIndexText()
    {
        this.IndexCount = this.ViewModel.FluidUI.DataBlock.AddFluidUIIndexTextToList();
        var fluidtext = this.ViewModel.FluidUI.DataBlock.IndexTextList.LastOrDefault();

        this.FluidUIIndexText = new FluidUIIndexTextViewModel(this.ViewModel, fluidtext);

        this.FluidUIIndexTexts.Add(this.FluidUIIndexText);

        this.OnPropertyChanged(nameof(this.FluidUIIndexTexts));
        this.OnPropertyChanged(nameof(this.FluidUIIndexText));
    }

    [RelayCommand]
    private void DeleteIndexText()
    {

        var index = this.FluidUIIndexText.Index;
        var time = this.FluidUIIndexText.Time;
        var text = this.FluidUIIndexText.Text;

        if (this.ViewModel.FluidUI.DataBlock.IndexTextList.Any(x => (x.Index == index && x.Time.Equals(time))))
        {
            var model = this.ViewModel.FluidUI.DataBlock.IndexTextList.Where(x => (x.Index == index && x.Time.Equals(time))).FirstOrDefault();

            if (model != null)
            {
                this.ViewModel.FluidUI.DataBlock.IndexTextList.Remove(model);

                this.FluidUIIndexTexts = new();

                foreach (var item in this.ViewModel.FluidUI.DataBlock.IndexTextList)
                {
                    this.FluidUIIndexTexts.Add(new FluidUIIndexTextViewModel(this.ViewModel, item));
                }
            }
        }

        this.FluidUIIndexText = this.FluidUIIndexTexts.LastOrDefault();

        this.OnPropertyChanged(nameof(this.FluidUIIndexTexts));
        this.OnPropertyChanged(nameof(this.FluidUIIndexText));
    }

    [RelayCommand]
    private void ClearIndexTextList()
    {
        this.ViewModel.FluidUI.DataBlock.IndexTextList.Clear();
        this.FluidUIIndexTexts.Clear();
        this.OnPropertyChanged(nameof(this.ViewModel.FluidUI));
        this.OnPropertyChanged(nameof(this.FluidUIIndexTexts));
    }
}

// EOF