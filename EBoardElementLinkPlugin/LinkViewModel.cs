namespace EBoardElementPluginLinker;

using CommunityToolkit.Mvvm.ComponentModel;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.Models.DataSets;
using EBoardSDK.Plugins;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public partial class LinkViewModel : EBoardElementPluginBaseViewModel
{
    [ObservableProperty]
    private string epicText = "this is the most epic text in the entire existance.";

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

    //[ObservableProperty]
    //private string pluginHeader = "Linker Element";

    //[ObservableProperty]
    //private string pluginName = new ElementPluginMetadata().ElementPluginName;    

    public override Task Load(string path, IElementDataSet elementDataSet)
    {
        return Task.CompletedTask;
    }

    public override Task Save(string path, IElementDataSet elementDataSet)
    {
        return Task.CompletedTask;
    }
}
