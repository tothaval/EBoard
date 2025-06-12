// <copyright file="SummonerViewModel.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK.Plugins.Tools.Summoner;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.Plugins.Elements.StandardText;
using EBoardSDK.SharedMethods;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public partial class SummonerViewModel : EBoardElementPluginBaseViewModel, IElementBackgroundImage
{
    [ObservableProperty]
    private string userCommandString = ">";

    partial void OnUserCommandStringChanged(string value)
    {
    }

    private double Angle { get; set; }

    [ObservableProperty]
    private IPlugin summonee;

    //public PluginDataSet PluginDataSet { get; set; } = new PluginDataSet();

    [ObservableProperty]
    private string imagePath;

    partial void OnImagePathChanged(string value)
    {
        ChangeElementBackgroundToImage(BrushTargets.Background, value);
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
            //Summonee.Height = value;
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
            //Summonee.Width = value;
        }

        UpdateContentWidth(value);
    }

    /// <summary>
    /// Gets or sets until a real brushManagement architecture is implemented, this serves as a mockup solution
    ///
    /// a real brushManagement architecture should check a folder for certain files or a file, in which
    /// data on plugins is stored, then all that data needs to be loaded and instanciated if
    /// necessary during app start
    ///
    /// the ui should build menuitems dynamically from that data
    /// </summary>
    public List<string> PluginsCategoryElements { get; set; } = ["StandardText"];

    /// <summary>
    /// Gets or sets until a real brushManagement architecture is implemented, this serves as a mockup solution
    /// </summary>
    public List<string> PluginsCategoryShapes { get; set; } = ["Ellipse", "Rectangle"];

    /// <summary>
    /// Gets or sets until a real brushManagement architecture is implemented, this serves as a mockup solution
    /// </summary>
    public List<string> PluginsCategoryTools { get; set; } = [
        "SessionUptimeClock", "EmptyLinear", "EmptyRadial", "Summoner", "Uptime"];

    public override bool NoDefaultBorders { get; } = false;

    public override PluginCategories PluginCategory => PluginCategories.Tool;

    public override ImageBrush PluginLogo { get; set; }

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(this.ElementPluginView)!;

    private string pluginHeader = "Summoner Element";

    public override string PluginHeader { get { return this.pluginHeader; } set { this.pluginHeader = value; } }

    private string pluginName = "Summoner";

    public override string PluginName { get { return this.pluginName; } set { this.pluginName = value; } }

    public override string ElementPluginName => "Summoner";

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new();

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(SummonerView);

    public override Type ElementPluginViewModel => typeof(SummonerViewModel);

    public SummonerViewModel() => this.InstantiateProperties();

    public void ChangeElementBackgroundToImage(BrushTargets brushTargets, string path)
    {
        if (this.ImagePath != null && this.ImagePath != string.Empty)
        {
            this.Summonee?.ApplyBrush(new SharedMethod_UI().ChangeBackgroundToImage(this.Summonee.BrushManagement.Background, this.ImagePath), Enums.BrushTargets.Background);
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

        if (this.PluginsCategoryElements.Contains(commandString))
        {
            category = "Elements";

            return true;
        }

        if (this.PluginsCategoryShapes.Contains(commandString))
        {
            category = "Shapes";

            return true;
        }

        if (this.PluginsCategoryTools.Contains(commandString))
        {
            category = "Tools";

            return true;
        }

        category = string.Empty;

        return false;
    }

    [RelayCommand]
    private void DeleteElement(object s)
    {
        this.Summonee = null;
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
        //this.Width = double.NaN;
        //this.Height = double.NaN;

        string command = this.UserCommandString.Substring(1);

        if (this.UserCommandString.StartsWith(">"))
        {
            if (this.CommandStringValidator(command, out string category))
            {
                /// am besten separate brushManagement category listen führen, diese vergleichen und wo treffer,
                /// da instanzieren.
                IPlugin? plugin = PluginFactory.GetPluginByCommand(category, command);

                if (plugin is not null)
                {
                    this.Summonee = plugin;
                    return;
                }
            }

            this.Summonee = new StandardTextViewModel() { Text = $"unknown brushManagement '{command}' called" };
            return;
        }

        this.Summonee = new StandardTextViewModel() { Text = $"unrecognized command" };
    }

    private void InstantiateProperties()
    {
        this.BorderManagement = new BorderManagement();
        this.BrushManagement = new BrushManagement();
    }

    public override Task<EBoardFeedbackMessage> Load(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty load call" });
    }

    public override Task<EBoardFeedbackMessage> Save(string path)
    {
        //this.PluginDataSet.References.Add(new("Type", this.Summonee?.Plugin?.GetType().FullName));
        //this.PluginDataSet.References.Add(new("Name", this.Summonee?.PluginName));
        //this.PluginDataSet.References.Add(new("Header", this.Summonee?.PluginHeader));

        //string contentDataPath = Path.Combine(path, linkDataFileName);

        //var model = new LinkModel() { LinkTargetName = this.LinkTargetName, LinkTargetPath = this.LinkTargetPath };

        //var serializationResult = await new SharedMethod_Plugins().SerializeConfigFiles(model, contentDataPath);

        //if (!serializationResult.TaskResult.Equals(EBoardTaskResult.Success))
        //{
        //    // TODO do stuff
        //}

        //return serializationResult;

        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty save call" });
    }

    [RelayCommand]
    private void ResetImage()
    {
        this.ImagePath = string.Empty;

        this.Summonee.ApplyBrush(new SharedMethod_UI().ImagePathErrorDefaultBrush, Enums.BrushTargets.Background);
    }

    [RelayCommand]
    private void SetImage()
    {
        this.ImagePath = new SharedMethod_UI().SetBackgroundImage(this.ImagePath);
    }

    private void UpdateContentHeight(int height)
    {
        if (this.Summonee is not null)
        {
            //this.Summonee.Height = height;
        }
    }

    private void UpdateContentWidth(int width)
    {
        if (this.Summonee is not null)
        {
            //this.Summonee.Width = width;
        }
    }

    private void UpdateCornerRadius(int value)
    {
        if (this.Summonee is not null)
        {
            // TODO update implementation
            //Summonee.BorderManagement.CornerRadiusValue = value;
        }
    }

    private void UpdateRotation(int rotationAngle)
    {
        this.RotateTransformValue = new RotateTransform(rotationAngle * -1);

        this.TransformOriginPoint = new Point(0.5, 0.5);

        //this.Width = double.NaN;
    }
}// EOF