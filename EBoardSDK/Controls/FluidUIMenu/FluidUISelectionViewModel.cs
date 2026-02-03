// <copyright file="FluidUISelectionViewModel.cs" company=".">
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
namespace EBoardSDK.Controls.FluidUIMenu;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Enums;
using EBoardSDK.Models;
using EBoardSDK.ViewModels;
using System;

public partial class FluidUISelectionViewModel : ObservableObject
{
    private FluidUIBaseViewModel viewModel;

    private Action? buttonClickAction;

    [ObservableProperty]
    private bool allConfiguration = true;

    [ObservableProperty]
    private bool dataConfiguration;

    [ObservableProperty]
    private bool designConfiguration;

    [ObservableProperty]
    private bool fontConfiguration;

    [ObservableProperty]
    private bool sizeConfiguration;

    [ObservableProperty]
    private bool standConfiguration;

    [ObservableProperty]
    private FluidUIConfigurationSetting all = new(ConfigurationTargets.All) { Selected = true };

    [ObservableProperty]
    private FluidUIConfigurationSetting dataBlock = new(ConfigurationTargets.DataBlock) { Selected = false };

    [ObservableProperty]
    private FluidUIConfigurationSetting design = new(ConfigurationTargets.Design) { Selected = false };

    [ObservableProperty]
    private FluidUIConfigurationSetting font = new(ConfigurationTargets.Font) { Selected = false };

    [ObservableProperty]
    private FluidUIConfigurationSetting size = new(ConfigurationTargets.Size) { Selected = false };

    [ObservableProperty]
    private FluidUIConfigurationSetting stand = new(ConfigurationTargets.Stand) { Selected = false };

    /// <summary>
    /// Initializes a new instance of the <see cref="FluidUISelectionViewModel"/> class.
    /// </summary>
    /// <param name="viewModel"></param>
    /// <param name="buttonClickAction"></param>
    public FluidUISelectionViewModel(FluidUIBaseViewModel viewModel, Action? buttonClickAction)
    {
        this.viewModel = viewModel;
        this.buttonClickAction = buttonClickAction;

        this.OnPropertyChanged(nameof(this.ViewModel));
    }

    public FluidUIBaseViewModel ViewModel => this.viewModel;

    partial void OnAllConfigurationChanging(bool oldValue, bool newValue)
    {
        if (newValue)
        {
            this.dataConfiguration = false;
            this.designConfiguration = false;
            this.fontConfiguration = false;
            this.sizeConfiguration = false;
            this.standConfiguration = false;
        }
    }

    partial void OnAllConfigurationChanged(bool value)
    {
        this.All.Selected = value;

        if (value)
        {
            this.OnPropertyChanged(nameof(this.DataConfiguration));
            this.OnPropertyChanged(nameof(this.DesignConfiguration));
            this.OnPropertyChanged(nameof(this.FontConfiguration));
            this.OnPropertyChanged(nameof(this.SizeConfiguration));
            this.OnPropertyChanged(nameof(this.StandConfiguration));
        }
    }

    partial void OnDataConfigurationChanged(bool value)
    {
        this.DataBlock.Selected = value;

        this.SetAllConfigurationFalse();
    }

    partial void OnDesignConfigurationChanged(bool value)
    {
        this.Design.Selected = value;
        this.SetAllConfigurationFalse();
    }

    partial void OnFontConfigurationChanged(bool value)
    {
        this.Font.Selected = value;
        this.SetAllConfigurationFalse();
    }

    partial void OnSizeConfigurationChanged(bool value)
    {
        this.Size.Selected = value;
        this.SetAllConfigurationFalse();
    }

    partial void OnStandConfigurationChanged(bool value)
    {
        this.Stand.Selected = value;
        this.SetAllConfigurationFalse();
    }

    private void SetAllConfigurationFalse()
    {
        if (this.AllConfiguration)
        {
            this.AllConfiguration = false;
        }
    }

    [RelayCommand]
    private async void ApplyFluidUIConfiguration()
    {
        this.buttonClickAction?.Invoke();
    }
}

// EOF