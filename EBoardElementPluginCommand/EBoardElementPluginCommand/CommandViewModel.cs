namespace EBoardElementPluginCommand;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.Models.DataSets;
using EBoardSDK.Plugins;

using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

public partial class CommandViewModel : EBoardElementPluginBaseViewModel
{
    [ObservableProperty]
    private string stdIn = string.Empty;


    [ObservableProperty]
    private string stdOut = string.Empty;


    [ObservableProperty]
    private string epicText = "this is the 2nd most epic text in the entire existance.";

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(ElementPluginView)!;

    private string pluginHeader = "Command Element";
    public override string PluginHeader { get { return pluginHeader; } set { pluginHeader = value; } }

    private string pluginName = "Command";
    public override string PluginName { get { return pluginName; } set { pluginName = value; } }

    public override string ElementPluginName => "Command";

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new() { Source = new Uri("/EBoardElementPluginCommand;component/ElementPluginResourceDictionary.xaml", uriKind: UriKind.Relative) };

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(CommandView);

    public override Type ElementPluginViewModel => typeof(CommandViewModel);

    [RelayCommand]
    private void EnterKeyPressed(object s)
    {
        StdOut = string.Join("\n", StdOut, s.ToString());
    }

    public override Task Load(string path, IElementDataSet elementDataSet)
    {
        return Task.CompletedTask;
    }

    public override Task Save(string path, IElementDataSet elementDataSet)
    {
        return Task.CompletedTask;
    }
}
