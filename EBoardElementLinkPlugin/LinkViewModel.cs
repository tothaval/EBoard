namespace EBoardElementPluginLinker;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK;
using EBoardSDK.Interfaces;
using EBoardSDK.Plugins;
using EBoardSDK.Plugins.Elements.StandardText;
using EBoardSDK.SharedMethods;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;

public partial class LinkViewModel : EBoardElementPluginBaseViewModel, ICollectiveClickableObject
{
    private readonly string linkDataFileName = "linkdata.xml";

    [ObservableProperty]
    private string epicText = "this is the most epic text in the entire existance.";

    [ObservableProperty]
    private string linkStatusText;

    [ObservableProperty]
    private string linkTargetPath;

    [ObservableProperty]
    private string linkTargetName;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsLinkEmpty))]
    private bool isLinked = false;

    public bool IsLinkEmpty => !IsLinked;

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(ElementPluginView)!;

    private string pluginHeader = "Link Element";

    public override string PluginHeader { get { return pluginHeader; } set { pluginHeader = value; } }

    private string pluginName = "Link";

    public override string PluginName { get { return pluginName; } set { pluginName = value; } }

    public override string ElementPluginName => "Linker";

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new() { Source = new Uri("/EBoardElementPluginLinker;component/ElementPluginResourceDictionary.xaml", uriKind: UriKind.Relative) };

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(LinkView);

    public override Type ElementPluginViewModel => typeof(LinkViewModel);

    public Action CollectiveClickEvent => ExecuteOnClick;

    private void ExecuteOnClick()
    {
        if (IsLinked)
        {
            ExecuteLinkTarget();
            return;
        }

        //Link();
    }

    //[ObservableProperty]
    //private string pluginHeader = "Linker Element";

    //[ObservableProperty]
    //private string pluginName = new ElementPluginMetadata().ElementPluginName;    

    [RelayCommand]
    private void ExecuteLinkTarget()
    {
        try
        {
            ProcessStartInfo start = new ProcessStartInfo(LinkTargetPath)
            {
                UseShellExecute = true
            };
            Process.Start(start);
        }
        catch
        {
            //if (btn.ToolTip.ToString() != null)
            //{
            //    MessageBox.Show(text.pathExcetptionMessage().ToString());
            //}
        }
    }


    [RelayCommand]
    private void Link()
    {
        Microsoft.Win32.OpenFileDialog setPath = new Microsoft.Win32.OpenFileDialog();
        setPath.InitialDirectory = Environment.GetEnvironmentVariable("userdir");
        setPath.Filter = "files (*.*)|*.*";
        setPath.FilterIndex = 2;
        setPath.RestoreDirectory = true;

        if (setPath.ShowDialog() == true)
        {
            LinkFile(setPath.FileName);

            //viewModel.ImagePath = setPath.FileName;
        }
    }

    private void LinkFile(string fileName, string linkName = null)
    {
        try
        {
            var fileInfo = new FileInfo(fileName);

            if (fileInfo.Exists)
            {
                this.LinkTargetName = linkName ?? fileInfo.Name;
                this.LinkTargetPath = fileInfo.FullName;

                IsLinked = fileInfo.Exists;
            }
        }
        catch (Exception)
        {
            Reset();
        }
    }

    [RelayCommand]
    private void Reset()
    {
        IsLinked = false;
        LinkTargetName = string.Empty;
        LinkTargetPath = string.Empty;

        LinkStatusText = $"unlinked";
    }

    public override async Task<EBoardFeedbackMessage> Load(string path)
    {
        if (new DirectoryInfo(path).Exists)
        {
            path = Path.Combine(path, linkDataFileName);
        }

        try
        {
            var data = await new SharedMethod_Plugins().DeserializeConfigFiles<LinkModel>(path);

            if (data != null)
            {
                LinkFile(data.LinkTargetPath, data.LinkTargetName);

                return new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = $"deserialized {path}" };
            }
        }
        catch (Exception ex)
        {
            return new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Exception, ResultMessage = ex.Message };
        }

        return new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Unknown, ResultMessage = "" };
    }

    public async override Task<EBoardFeedbackMessage> Save(string path)
    {
        EBoardFeedbackMessage? serializationResult = null;

        var model = new LinkModel() { LinkTargetName = this.LinkTargetName, LinkTargetPath = this.LinkTargetPath };

        if (new DirectoryInfo(path).Exists)
        {
            path = Path.Combine(path, linkDataFileName);            
        }

        serializationResult = await new SharedMethod_Plugins().SerializeConfigFiles(model, path);

        if (!serializationResult.TaskResult.Equals(EBoardTaskResult.Success))
        {
            // TODO do stuff
        }

        return serializationResult!;
    }
}
