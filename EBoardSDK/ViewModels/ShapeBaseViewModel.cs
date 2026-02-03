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
using EBoardSDK.Controls.FluidUIMenu;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.Plugins;
using EBoardSDK.Utilities;
using EBoardSDK.Utilities.Factories;
using System;
using System.Windows;
using System.Windows.Media;

public abstract partial class ShapeBaseViewModel : PluginBaseViewModel, IDisposable
{
    private FluidUIBaseViewModel viewModel = new ();

    private FluidUIMenuViewModel? fluidUIMenuViewModel;

    [ObservableProperty]
    private double strokeThickness = 1.0;

    [ObservableProperty]
    private bool hideElement = true;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShapeBaseViewModel"/> class.
    /// </summary>
    protected ShapeBaseViewModel()
    {
        this.ScreenInstantiationConstraints = new InstantiationAndCopyConstraints(
            elementInstantiationPolicy: InstantiationPolicy.Unconstrained,
            copyConstraints: CopyConstraints.FullCopy);
    }

    public FluidUIBaseViewModel ViewModel => this.viewModel;

    public FluidUIMenuViewModel? FluidUIMenuViewModel => this.fluidUIMenuViewModel;

    /// <inheritdoc/>
    public override void RefreshInitialization()
    {
        if (this.ElementViewModel != null)
        {
            if (this.ViewModel == null)
            {
                this.viewModel = new FluidUIBaseViewModel();
            }

            this.ChangeElementVisibility(this.HideElement);

            this.ElementViewModel?.Redraw();

            this.OnPropertyChanged(nameof(this.ElementViewModel));
            this.OnPropertyChanged(nameof(this.ViewModel));
            this.OnPropertyChanged(nameof(this.FluidUIMenuViewModel));
            this.OnPropertyChanged(nameof(this.HideElement));
        }
    }

    public void SetFluidUIContext(IFluidUIContext fluidUIContext)
    {
        if (this.ViewModel == null)
        {
            this.viewModel = new ();
            this.OnPropertyChanged(nameof(this.ViewModel));
        }

        if (fluidUIContext == null)
        {
            return;
        }

        this.ViewModel?.SetFluidUI(fluidUIContext);

        // TODO rebuild shape, and develop new shape ui menu
        //if (this.ElementViewModel != null)
        //{
        //    this.ViewModel?.SetFluidUIMenuViewModel(new FluidUIMenuViewModel(this.ViewModel, this.ElementViewModel, this.ElementViewModel?.ScreenViewModel, fluidUIContextHasStand: true));
        //}

        this.OnPropertyChanged(nameof(this.ElementViewModel));
        this.OnPropertyChanged(nameof(this.ViewModel));
        this.OnPropertyChanged(nameof(this.FluidUIMenuViewModel));
    }

    public void SetFluidUIViewModel(FluidUIBaseViewModel? fluidUIViewModel)
    {
        this.viewModel = fluidUIViewModel ?? new FluidUIBaseViewModel();

        this.OnPropertyChanged(nameof(this.ElementViewModel));
        this.OnPropertyChanged(nameof(this.ViewModel));
        this.OnPropertyChanged(nameof(this.FluidUIMenuViewModel));
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
    }

    /// <inheritdoc/>
    public override async Task<EboardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await new SDKDataManager().LoadPluginContent<FluidUIContext>(path);

            if (data != null)
            {
                var fluidUIViewModel = new FluidUIBaseViewModel();
                fluidUIViewModel.SetFluidUI(data);

                this.SetFluidUIViewModel(fluidUIViewModel);

                this.RefreshInitialization();

                return FeedbackMessageFactory.Success($"deserialized {path}");
            }
        }
        catch (Exception ex)
        {
            return FeedbackMessageFactory.Exception(ex.Message);
        }

        return FeedbackMessageFactory.Unknown($"Load() T {typeof(FluidUIContext).FullName}");
    }

    /// <inheritdoc/>
    public override async Task<EboardFeedbackMessage> Save(string path)
    {
        var model = this.ViewModel.FluidUI;

        return await new SDKDataManager().SavePluginContent(model, path);
    }

    private void ChangeElementVisibility(bool hideElementsChanged)
    {
        if (hideElementsChanged)
        {
            if (this.ElementViewModel != null)
            {
                if (this.ElementViewModel.FluidUI.Design != null)
                {
                    this.ElementViewModel.FluidUI.Design.Background = FluidUIDesignDefaultPropertyFactory.TransparentSolidColorBrush;
                }

                this.ElementViewModel.Redraw();
            }

            return;
        }

        if (this.ElementViewModel != null)
        {
            if (this.ElementViewModel.FluidUI.Design != null)
            {
                this.ElementViewModel.FluidUI.Design.Background = FluidUIDesignDefaultPropertyFactory.BackgroundDefaultSolidColorBrush;
            }

            this.ElementViewModel.Redraw();
        }
    }

    partial void OnHideElementChanged(bool value)
    {
        this.ChangeElementVisibility(value);
    }

    [RelayCommand]
    private void DeleteElement(object s)
    {
        this.ElementViewModel?.ScreenViewModel.RemoveElement(this.ElementViewModel);
    }
}

// EOF