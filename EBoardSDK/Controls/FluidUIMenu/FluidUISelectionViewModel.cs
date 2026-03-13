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
using EBoardSDK.SharedMethods;
using EBoardSDK.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class FluidUISelectionViewModel : ObservableObject
{
    private EboardFluidUIBaseViewModel viewModel;

    private Action? buttonClickAction;

    [ObservableProperty]
    private bool allConfiguration = true;

    [ObservableProperty]
<<<<<<< Updated upstream
    private bool dataConfiguration = false;
=======
    private bool nothingConfiguration;

    [ObservableProperty]
    private bool dataConfiguration;
>>>>>>> Stashed changes

    [ObservableProperty]
    private bool designConfiguration = false;

    [ObservableProperty]
    private bool fontConfiguration = false;

    [ObservableProperty]
    private bool sizeConfiguration = false;

    [ObservableProperty]
    private bool standConfiguration = false;

    [ObservableProperty]
<<<<<<< Updated upstream
    private ConfigurationTargets configurationTarget = ConfigurationTargets.All;
=======
    private FluidUIConfigurationSetting all = new(ConfigurationTargets.All) { Selected = true };

    [ObservableProperty]
    private FluidUIConfigurationSetting nothing = new(ConfigurationTargets.Nothing) { Selected = false };

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
>>>>>>> Stashed changes

    /// <summary>
    /// Initializes a new instance of the <see cref="FluidUISelectionViewModel"/> class.
    /// </summary>
    /// <param name="viewModel"></param>
    /// <param name="buttonClickAction"></param>
    public FluidUISelectionViewModel(EboardFluidUIBaseViewModel viewModel, Action? buttonClickAction)
    {
        this.viewModel = viewModel;
        this.buttonClickAction = buttonClickAction;

        this.OnPropertyChanged(nameof(this.ViewModel));
    }

<<<<<<< Updated upstream
    public EboardFluidUIBaseViewModel ViewModel => this.viewModel;

    partial void OnAllConfigurationChanged(bool value)
    {
        if (value)
        {
            this.ConfigurationTarget = ConfigurationTargets.All;

            this.DataConfiguration = false;
            this.DesignConfiguration = false;
            this.FontConfiguration = false;
            this.SizeConfiguration = false;
            this.StandConfiguration = false;
=======
    public FluidUIBaseViewModel ViewModel => this.viewModel;

    private void SetAllConfigurationFalse()
    {
        if (this.AllConfiguration)
        {
            this.AllConfiguration = false;
        }

        if (this.NothingConfiguration)
        {
            this.NothingConfiguration = false;
        }
    }

    private void SetEveryConfigurationToFalse()
    {
        this.NothingConfiguration = false;
        this.DataConfiguration = false;
        this.DesignConfiguration = false;
        this.FontConfiguration = false;
        this.SizeConfiguration = false;
        this.StandConfiguration = false;
    }

    partial void OnAllConfigurationChanging(bool oldValue, bool newValue)
    {
        if (newValue)
        {
            this.SetEveryConfigurationToFalse();
        }
    }

    partial void OnAllConfigurationChanged(bool value)
    {
        this.All.Selected = value;
    }

    partial void OnNothingConfigurationChanged(bool value)
    {
        if (value)
        {
            this.SetEveryConfigurationToFalse();
            this.SetAllConfigurationFalse();

            this.nothingConfiguration = value;
            this.OnPropertyChanged(nameof(this.NothingConfiguration));
>>>>>>> Stashed changes
        }
    }

    partial void OnDataConfigurationChanged(bool value)
    {
        if (value)
        {
            this.ConfigurationTarget = ConfigurationTargets.DataBlock;
            this.SetSaveAllConfiguration(false);
        }
    }

    partial void OnDesignConfigurationChanged(bool value)
    {
        if (value)
        {
            this.ConfigurationTarget = ConfigurationTargets.Design;
            this.SetSaveAllConfiguration(false);
        }
    }

    partial void OnFontConfigurationChanged(bool value)
    {
        if (value)
        {
            this.ConfigurationTarget = ConfigurationTargets.Font;
            this.SetSaveAllConfiguration(false);
        }
    }

    partial void OnSizeConfigurationChanged(bool value)
    {
        if (value)
        {
            this.ConfigurationTarget = ConfigurationTargets.Size;
            this.SetSaveAllConfiguration(false);
        }
    }

    partial void OnStandConfigurationChanged(bool value)
    {
        if (value)
        {
            this.ConfigurationTarget = ConfigurationTargets.Stand;
            this.SetSaveAllConfiguration(false);
        }
    }

<<<<<<< Updated upstream
    private void SetSaveAllConfiguration(bool value)
    {
        this.AllConfiguration = value;
    }

=======
>>>>>>> Stashed changes
    [RelayCommand]
    private async void ApplyFluidUIConfiguration()
    {
        this.buttonClickAction?.Invoke();
    }
}

// EOF