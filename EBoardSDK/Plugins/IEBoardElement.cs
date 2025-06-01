using EBoardSDK.Interfaces;
using System.Reflection;
using System.Windows;

namespace EBoardSDK.Plugins
{
    public interface IEBoardElement : IPlugin
    {
        public string ElementPluginName { get; }

        public Assembly? ElementPluginAssembly { get; }

        public ResourceDictionary ResourceDictionary { get; }

        public Type? ElementPluginModel { get; }

        public Type ElementPluginView { get; }

        public Type ElementPluginViewModel { get; }
    }
}
