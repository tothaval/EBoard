using CommunityToolkit.Mvvm.ComponentModel;
using EBoardSDK.Enums;
using EBoardSDK.Models;
using Serilog;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EBoardSDK.Plugins
{
    public abstract partial class EBoardElementPluginBaseViewModel : ObservableObject, IEBoardElement
    {
        private BorderManagement _BorderManagement = new();

        public BorderManagement BorderManagement
        {
            get { return this._BorderManagement; }

            set
            {
                this._BorderManagement = value;
                this.OnPropertyChanged(nameof(this.BorderManagement));
                this.OnPropertyChanged(nameof(this.Plugin));
            }
        }

        private BrushManagement _BrushManagement = new();

        public BrushManagement BrushManagement
        {
            get { return this._BrushManagement; }

            set
            {
                this._BrushManagement = value;
                this.OnPropertyChanged(nameof(this.BrushManagement));
                this.OnPropertyChanged(nameof(this.Plugin));
            }
        }

        public abstract bool NoDefaultBorders { get; }

        public abstract ImageBrush PluginLogo { get; set; }

        public abstract UserControl Plugin { get; }

        public abstract PluginCategories PluginCategory { get; }

        public abstract string PluginHeader { get; set; }

        public abstract string PluginName { get; set; }

        public abstract string ElementPluginName { get; }

        public abstract Assembly? ElementPluginAssembly { get; }

        public abstract Type? ElementPluginModel { get; }

        public abstract Type ElementPluginView { get; }

        public abstract Type ElementPluginViewModel { get; }

        public abstract ResourceDictionary ResourceDictionary { get; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(BorderManagement))]
        private double height;

        partial void OnHeightChanged(double value)
        {
            BorderManagement.Height = value;
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(BorderManagement))]
        private double width;

        partial void OnWidthChanged(double value)
        {
            BorderManagement.Width = value;
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

        public bool ApplyRedraw()
        {
            this.OnPropertyChanged(nameof(this.BorderManagement));
            this.OnPropertyChanged(nameof(this.BrushManagement));
            this.OnPropertyChanged(nameof(this.Plugin));

            return true;
        }

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

        public abstract Task<EBoardFeedbackMessage> Load(string path);

        public abstract Task<EBoardFeedbackMessage> Save(string path);
    }
}
