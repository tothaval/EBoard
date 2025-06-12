// <copyright file="EBoardElementPluginBaseViewModel.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK.Plugins;

using CommunityToolkit.Mvvm.ComponentModel;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using Serilog;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

public abstract partial class EBoardElementPluginBaseViewModel : ObservableObject, IPlugin
{
    private BorderManagement borderManagement = new();

    private BrushManagement brushManagement = new();

    public BorderManagement BorderManagement
    {
        get { return this.borderManagement; }

        set
        {
            this.borderManagement = value;
            this.OnPropertyChanged(nameof(this.BorderManagement));
            this.OnPropertyChanged(nameof(this.Plugin));
        }
    }

    public BrushManagement BrushManagement
    {
        get { return this.brushManagement; }

        set
        {
            this.brushManagement = value;
            this.OnPropertyChanged(nameof(this.BrushManagement));
            this.OnPropertyChanged(nameof(this.Plugin));
        }
    }

    public abstract Assembly? ElementPluginAssembly { get; }

    public abstract Type? ElementPluginModel { get; }

    public abstract string ElementPluginName { get; }

    public abstract Type ElementPluginView { get; }

    public abstract Type ElementPluginViewModel { get; }

    public ElementScreenIntegrationConstraints? ElementScreenIntegrationConstraints { get; set; } = new ElementScreenIntegrationConstraints(ElementInstantiationPolicy.ValueNotSet);

    public abstract bool NoDefaultBorders { get; }

    public abstract UserControl Plugin { get; }

    public abstract PluginCategories PluginCategory { get; }

    public abstract ImageBrush PluginLogo { get; set; }

    public abstract string PluginName { get; set; }

    public abstract string PluginHeader { get; set; }

    public abstract ResourceDictionary ResourceDictionary { get; }

    public bool ApplyBrush(Brush brush, BrushTargets brushTargets)
    {
        try
        {
            switch (brushTargets)
            {
                case BrushTargets.Background:
                    this.BrushManagement.Background = brush;
                    this.OnPropertyChanged(nameof(this.BrushManagement.Background));

                    break;
                case BrushTargets.Border:
                    this.BrushManagement.Border = brush;
                    this.OnPropertyChanged(nameof(this.BrushManagement.Border));
                    break;
                case BrushTargets.Foreground:
                    this.BrushManagement.Foreground = brush;
                    this.OnPropertyChanged(nameof(this.BrushManagement.Foreground));
                    break;
                case BrushTargets.Highlight:
                    this.BrushManagement.Highlight = brush;
                    this.OnPropertyChanged(nameof(this.BrushManagement.Highlight));
                    break;
                default:
                    break;
            }

            this.OnPropertyChanged(nameof(this.BrushManagement));
            this.OnPropertyChanged(nameof(this.Plugin));

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool ApplyRedraw()
    {
        this.OnPropertyChanged(nameof(this.BorderManagement));
        this.OnPropertyChanged(nameof(this.BrushManagement));
        this.OnPropertyChanged(nameof(this.Plugin));

        return true;
    }

    public async Task<bool> Initialize()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(this.PluginName))
            {
                return false;
            }

            var path = await new Runner().GetConfigPathsAsync();

            var resourcestring = Path.Combine(path.PluginFolder, $"{this.PluginName}_Logo.png");

            var imagefile = new FileInfo(resourcestring);

            if (!imagefile.Exists)
            {
                return false;
            }

            var imagesource = new BitmapImage(new Uri(imagefile.FullName));

            this.PluginLogo = new ImageBrush(imagesource);

            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            //throw;
        }

        return false;
    }

    public abstract Task<EBoardFeedbackMessage> Load(string path);

    public abstract Task<EBoardFeedbackMessage> Save(string path);
}
