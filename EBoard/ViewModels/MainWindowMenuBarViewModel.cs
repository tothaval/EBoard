/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  MainWindowMenuBarViewModel 
 * 
 *  view model for MainWindowMenuBarView, which has prototype element instantiation
 *  button and a prototype shape menu and a button to switch on or off the EBoardBrowserView
 */
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoard.Commands.ElementCreationCommands;
using EBoard.IOProcesses.DataSets;
using EBoard.Utilities.Factories;
using EBoardSDK.Interfaces;
using EBoardSDK.Plugins.Elements.StandardText;
using System.Windows.Input;
using System.Windows.Media.Media3D;

using EBoardElementPluginLinker;
using System.Collections.ObjectModel;
using EBoardSDK.Plugins;

namespace EBoard.ViewModels;

public partial class MainWindowMenuBarViewModel : ObservableObject
{

    // Properties & Fields
    #region Properties & Fields

    [ObservableProperty]
    private string title;


    [ObservableProperty]
    private bool eBoardBrowserSwitch;


    private MainViewModel _MainViewModel;
    public MainViewModel MainViewModel => _MainViewModel;

    #endregion



    // Commands
    #region Commands
    public ICommand InvokeElementCommand { get; }

    #endregion

    [ObservableProperty]
    private ObservableCollection<EBoardElementPluginBaseViewModel> plugins;

    public MainWindowMenuBarViewModel(MainViewModel mainViewModel, EboardConfig eboardConfig)
    {
        title = "EBoard";

        _MainViewModel = mainViewModel;

        Plugins = eboardConfig.Plugins;

        InvokeElementCommand = new InvokeElementCommand(mainViewModel);
    }

    [RelayCommand]
    private void InvokePlugin(object s)
    {
        if (s is Type)
        {
            var p = s as Type;
            var plugin = Activator.CreateInstance(p) as EBoardElementPluginBaseViewModel;

                var interfaces = plugin?.GetType().GetInterfaces();

            if (_MainViewModel.EBoardBrowserViewModel.SelectedEBoard != null &&
                plugin != null &&
                interfaces != null &&
                interfaces.Any(x => x.Name.Equals(nameof(IPlugin))))
                {               

                IElementDataSet newElementDataSet = new ElementDataSet();

                newElementDataSet = ElementDataSetFactory.GetElementDataSet(
                    plugin: plugin
                    );

                ElementViewModel evm = new ElementViewModel(
                    _MainViewModel.EBoardBrowserViewModel.SelectedEBoard,
                    newElementDataSet
                );

                if (!App.Current.Resources.MergedDictionaries.Contains(plugin.ResourceDictionary))
                {
                    App.Current.Resources.MergedDictionaries.Add(plugin.ResourceDictionary);
                }

                _MainViewModel.EBoardBrowserViewModel.SelectedEBoard.AddElement(evm);
            }
        }
    }

    [RelayCommand]
    private void InvokeEpic()
    {
        if (_MainViewModel.EBoardBrowserViewModel.SelectedEBoard != null)
        {
            IElementDataSet newElementDataSet = new ElementDataSet();

            newElementDataSet = ElementDataSetFactory.GetElementDataSet(
                plugin: new EBoardElementPluginLinker.LinkViewModel()
                );

            ElementViewModel evm = new ElementViewModel(
                _MainViewModel.EBoardBrowserViewModel.SelectedEBoard,
                newElementDataSet
            );

            App.Current.Resources.MergedDictionaries.Add(new EBoardElementPluginLinker.LinkViewModel().ResourceDictionary);

            _MainViewModel.EBoardBrowserViewModel.SelectedEBoard.AddElement(evm);
        }
    }
}
// EOF