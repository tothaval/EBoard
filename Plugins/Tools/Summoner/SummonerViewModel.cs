using CommunityToolkit.Mvvm.ComponentModel;
using EBoard.Interfaces;
using EBoard.Models;
using EBoard.Plugins.Elements.StandardText;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using EBoard.ViewModels;
using EBoard.Utilities.Factories;
using System.Reflection.Metadata.Ecma335;
using EBoard.Utilities.SharedMethods;
using EBoard.Views;
using EBoard.IOProcesses.DataSets;

namespace EBoard.Plugins.Tools.Summoner;

public partial class SummonerViewModel : PluginViewModel, IElementSelection, IElementBackgroundImage
{

    // PlacementManagement für Sumonee adden, oder separat nur die Angle mit speichern per PluginDataStorage


    [ObservableProperty]
    private IPlugin summonee;


    [ObservableProperty]
    private string userCommandString = ">";

    partial void OnUserCommandStringChanged(string value)
    {



    }

    public bool IsSelected { get; set; }





    private double Angle {  get; set; }


    [ObservableProperty]
    private string imagePath;
    partial void OnImagePathChanged(string value)
    {
        ChangeElementBackgroundToImage();
    }


    [ObservableProperty]
    private int rotationAngleValue;

    partial void OnRotationAngleValueChanging(int oldValue, int newValue)
    {
        if (oldValue != newValue)
        {
            UpdateRotation(newValue);
        }
    }


    [ObservableProperty]
    private RotateTransform rotateTransformValue;

    partial void OnRotateTransformValueChanged(RotateTransform value)
    {
        Angle = RotationAngleValue;
    }


    [ObservableProperty]
    private Point transformOriginPoint;


    [ObservableProperty]
    private int cornerRadiusValueSummonee;

    partial void OnCornerRadiusValueSummoneeChanged(int value)
    {
        UpdateCornerRadius(value);
    }


    [ObservableProperty]
    private int heightValue;

    partial void OnHeightValueChanged(int value)
    {
        TransformOriginPoint = new Point(0, 0);

        if (Summonee is not null)
        {
            Summonee.Height = value;
        }

        UpdateContentHeight(value);
    }


    [ObservableProperty]
    private int widthValue;

    partial void OnWidthValueChanged(int value)
    {
        TransformOriginPoint = new Point(0, 0);

        if (Summonee is not null)
        {
            Summonee.Width = value;
        }

        UpdateContentWidth(value);
    }


    public SummonerViewModel()
    {
    }


    public void ChangeElementBackgroundToImage()
    {
        if (ImagePath != null && ImagePath != string.Empty)
        {
            Summonee?.ApplyBackgroundBrush(new SharedMethod_UI().ChangeBackgroundToImage(Summonee.BrushManagement.Background, ImagePath));
        }
    }


    /// <summary>
    /// until a real command architecture is implemented, this serves as a mockup solution
    /// 
    /// a real command architecture should be done in a separate class or using an api that
    /// handles all validations etc. it should also have several usercontrols for different
    /// authorization levels, f.e. Request(from aeui) could include some display with output(stderr, stdout, internal logging)
    /// and allow for call of operations on elements or other stuff, f.e. Admin could be used to handle stuff, that requires
    /// some sort of security clearance, all that has low priority atm
    /// 
    /// validation could also use onerrorinfo with community toolkit, but i need to look into that first
    /// </summary>
    private bool CommandStringValidator(string commandString, out string category)
    {
        if (commandString is null || commandString.Equals(string.Empty))
        {
            category = string.Empty;

            return false;
        }

        MainViewModel? mainViewModel = Application.Current.MainWindow.DataContext as MainViewModel;

        if (mainViewModel is not null)
        {
            if (mainViewModel.PluginsCategoryElements.Contains(commandString))
            {
                category = "Elements";

                return true;
            }

            if (mainViewModel.PluginsCategoryShapes.Contains(commandString))
            {
                category = "Shapes";

                return true;
            }

            if (mainViewModel.PluginsCategoryTools.Contains(commandString))
            {
                category = "Tools";

                return true;
            }
        }

        category = string.Empty;

        return false;
    }



    [RelayCommand]
    private void DeleteElement(object s)
    {
        Summonee = null;
    }


    /// <summary>
    /// until a real command architecture is implemented, this serves as a mockup solution
    /// 
    /// a real command architecture should be done in a separate class or using an api that
    /// handles all validations etc.
    /// 
    /// validation could also use onerrorinfo with community toolkit, but i need to look into that first
    /// </summary>
    [RelayCommand]
    private void ExecuteCommandString()
    {
        Width = double.NaN;
        Height = double.NaN;

        string command = UserCommandString.Substring(1);

        if (UserCommandString.StartsWith(">"))
        {
            if (CommandStringValidator(command, out string category))
            {
                /// am besten separate plugin category listen führen, diese vergleichen und wo treffer,
                /// da instanzieren.
                IPlugin? plugin = PluginFactory.GetPluginByCommand(category, command);

                if (plugin is not null)
                {
                    Summonee = plugin;
                    return;
                }
            }

            Summonee = new StandardTextViewModel() { Text = $"unknown plugin '{command}' called"};
            return;
        }

        Summonee = new StandardTextViewModel() { Text = $"unrecognized command"};

    }


    public override Task<bool> Load(string path, IPluginDataSet pluginDataSet)
    {
        PluginDataSet = pluginDataSet;


        PluginData pluginDataSummonee = PluginDataSet.PluginDataStorage.Find(x => x.Key.Equals("Summonee"));

        UserCommandString = $">{pluginDataSummonee.Value}";

        ExecuteCommandString();

        PluginData pluginDataPath = PluginDataSet.PluginDataStorage.Find(x => x.Key.Equals("Path"));

        BorderManagement = new BorderManagement(pluginDataSet.BorderDataSet);
        BrushManagement = new BrushManagement(pluginDataSet.BrushDataSet);

        PluginHeader = PluginDataSet.PluginHeader;
        PluginName = PluginDataSet.PluginName;

        return Task.FromResult(true);
    }



    [RelayCommand]
    private void ResetImage()
    {
        ImagePath = string.Empty;

        Summonee.ApplyBackgroundBrush(new SharedMethod_UI().ImagePathErrorDefaultBrush);
    }


    public override async Task<bool> OnEboardShutdown(string path)
    {
        PluginDataSet = new PluginDataSet(this);

        PluginDataSet.AddPluginData(new PluginData() { Key = "Path", Value = path });

        if (Summonee is not null)
        {
            PluginDataSet.AddPluginData(new PluginData() { Key = "Summonee", Value = Summonee.PluginName });
        }

        await Task.CompletedTask;

        return true;
    }


    [RelayCommand]
    public void Select()
    {
        IsSelected = !IsSelected;

        Summonee?.SelectionChange(IsSelected);
    }


    [RelayCommand]
    private void SetImage()
    {
        ImagePath = new SharedMethod_UI().SetBackgroundImage(ImagePath);
    }


    private void UpdateContentHeight(int height)
    {
        if (Summonee is not null)
        {
            Summonee.Height = height;
        }
    }


    private void UpdateContentWidth(int width)
    {
        if (Summonee is not null)
        {
            Summonee.Width = width;
        }
    }


    private void UpdateCornerRadius(int value)
    {
        if (Summonee is not null)
        {
            Summonee.CornerRadiusValue = value;
        }
    }


    private void UpdateRotation(int rotationAngle)
    {
        RotateTransformValue = new RotateTransform(rotationAngle * -1);

        TransformOriginPoint = new Point(0.5, 0.5);

        Width = double.NaN;
    }


}// EOF