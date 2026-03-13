// <copyright file="ShapeBaseViewModel.cs" company=".">
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
namespace EBoardSDK.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardConfigManager.Enums;
using EBoardConfigManager.Helper;
using EBoardSDK.Controls.FluidUIMenu;
<<<<<<< Updated upstream
using EBoardSDK.Interfaces;
=======
using EBoardSDK.Enums;
>>>>>>> Stashed changes
using EBoardSDK.Models;
using EBoardSDK.Plugins;
using System;

public abstract partial class ShapeBaseViewModel : EBoardElementPluginBaseViewModel, IDisposable
{
<<<<<<< Updated upstream
    private EboardFluidUIBaseViewModel viewModel = new();

    private FluidUIMenuViewModel fluidUIMenuViewModel;

=======
>>>>>>> Stashed changes
    [ObservableProperty]
    private double strokeThickness = 1.0;

    [ObservableProperty]
<<<<<<< Updated upstream
    private int duplicationCount = 1;
=======
    private bool hasStroke = true;
>>>>>>> Stashed changes

    /// <summary>
    /// Initializes a new instance of the <see cref="ShapeBaseViewModel"/> class.
    /// </summary>
    protected ShapeBaseViewModel()
    {
    }

<<<<<<< Updated upstream
    public EboardFluidUIBaseViewModel ViewModel => this.viewModel;

    public FluidUIMenuViewModel? FluidUIMenuViewModel => this.fluidUIMenuViewModel;

=======
    public FluidUIMenuViewModel? FluidUIMenuViewModel => this.fluidUIMenuViewModel;

    public void ApplyShapeModel(ShapeModel shapeModel)
    {
        this.HasStroke = shapeModel.HasStroke;
        this.StrokeThickness = shapeModel.StrokeThickness;

        this.SetFluidUI(shapeModel.FluidUIContext);
    }

    /// <inheritdoc/>
>>>>>>> Stashed changes
    public override void RefreshInitialization()
    {
        if (this.ElementViewModel != null)
        {
<<<<<<< Updated upstream
            this.SetFluidUIViewModel(this.ViewModel ?? new EboardFluidUIBaseViewModel());

            this.ViewModel?.SetFluidUIMenuViewModel(new FluidUIMenuViewModel(this.ViewModel, this.ElementViewModel, this.ElementViewModel?.EBoardViewModel, fluidUIContextHasStand: true));

=======
            if (this.FluidUIMenuViewModel == null)
            {
                this.CreateFluidUIMenuViewModel();
            }

>>>>>>> Stashed changes
            this.ElementViewModel?.Redraw();

            this.OnPropertyChanged(nameof(this.ElementViewModel));
            this.OnPropertyChanged(nameof(this.FluidUIMenuViewModel));
        }
    }

<<<<<<< Updated upstream
    public void SetFluidUIContext(IFluidUIContext fluidUIContext)
    {
        if (this.ViewModel == null)
        {
            this.viewModel = new();
            this.OnPropertyChanged(nameof(this.ViewModel));
        }

        this.ViewModel?.SetFluidUI(fluidUIContext);
        this.ViewModel?.SetFluidUIMenuViewModel(new FluidUIMenuViewModel(this.ViewModel, this.ElementViewModel, this.ElementViewModel?.EBoardViewModel, fluidUIContextHasStand: true));

        this.OnPropertyChanged(nameof(this.ElementViewModel));
        this.OnPropertyChanged(nameof(this.ViewModel));
        this.OnPropertyChanged(nameof(this.FluidUIMenuViewModel));
    }

    public void SetFluidUIViewModel(EboardFluidUIBaseViewModel? fluidUIViewModel)
    {
        this.viewModel = fluidUIViewModel ?? new EboardFluidUIBaseViewModel();

        if (this.ElementViewModel != null)
        {
            if (this.ElementViewModel.FluidUI.Design != null)
            {
                this.ElementViewModel.FluidUI.Design.Background = new SolidColorBrush(Colors.Transparent);
                this.ElementViewModel.FluidUI.Design.Foreground = new SolidColorBrush(Colors.Transparent);
                this.ElementViewModel.FluidUI.Design.Border = new SolidColorBrush(Colors.Transparent);
            }

            if (this.ElementViewModel.FluidUI.Size != null)
            {
                this.ElementViewModel.FluidUI.Size.Margin = new Thickness(0);
                this.ElementViewModel.FluidUI.Size.Padding = new Thickness(0);
                this.ElementViewModel.FluidUI.Size.BorderThickness = new Thickness(0);
                this.ElementViewModel.FluidUI.Size.CornerRadius = new CornerRadius(0);
            }

            if (this.ViewModel != null && this.ViewModel.FluidUI.Size != null)
            {
                this.ViewModel.FluidUI.Size.Margin = new Thickness(0);
                this.ViewModel.FluidUI.Size.Padding = new Thickness(0);
                this.ViewModel.FluidUI.Size.CornerRadius = new CornerRadius(0);
            }

            this.ElementViewModel.Redraw();
        }

        this.OnPropertyChanged(nameof(this.ElementViewModel));
        this.OnPropertyChanged(nameof(this.ViewModel));
        this.OnPropertyChanged(nameof(this.FluidUIMenuViewModel));
    }

    public void Dispose()
=======
    /// <inheritdoc/>
    public override void Dispose()
>>>>>>> Stashed changes
    {
    }

    public override async Task<EBoardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await Loader.LoadJsonFile<FluidUIContext>(path);

            if (data != null)
            {
<<<<<<< Updated upstream
                var fluidUIViewModel = new EboardFluidUIBaseViewModel();
                data.Design?.LoadBrushesFromColorData();
                fluidUIViewModel.SetFluidUI(data);
=======
                this.SetFluidUI(data);
>>>>>>> Stashed changes

                this.CreateFluidUIMenuViewModel();

                this.RefreshInitialization();

                return new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = $"deserialized {path}" };
            }
        }
        catch (Exception ex)
        {
            return new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Exception, ResultMessage = ex.Message };
        }

        return new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Unknown, ResultMessage = string.Empty };
    }

    public override async Task<EBoardFeedbackMessage> Save(string path)
    {
<<<<<<< Updated upstream
        EBoardFeedbackMessage? serializationResult = null;

        var model = this.ViewModel.FluidUI;
=======
        var model = this.FluidUI;
>>>>>>> Stashed changes

        var result = Saver.SaveJsonFile(path, model);

<<<<<<< Updated upstream
        return new EBoardFeedbackMessage()
        {
            ResultMessage = $"{path} :: saving eboard config: {result}",
            TaskResult = result.Equals(Result.Success) ? EBoardTaskResult.Success : EBoardTaskResult.Unknown,
        };
=======
    internal override void CreateFluidUIMenuViewModel()
    {
        if (this.fluidUIMenuViewModel == null)
        {
            this.SetFluidUIMenuViewModel(new FluidUIMenuViewModel(this, Enums.FluidUIStandSettings.NoStandContextArea));
        }
>>>>>>> Stashed changes
    }

    [RelayCommand]
    private void DeleteElement(object s)
    {
        if (this.ElementViewModel != null)
        {
            this.ElementViewModel.EBoardViewModel.RemoveElement(this.ElementViewModel);
        }
    }

    [RelayCommand]
    private void DuplicateNTimes()
    {
        if (this.EBoardViewModel != null)
        {
            this.ElementViewModel.EBoardViewModel.GetWindowMenuBarViewModel().InvokePluginNTimes(this.ElementPluginViewModel, this.DuplicationCount);
        }
    }
}

// EOF