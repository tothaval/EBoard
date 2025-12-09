namespace EEP_FSNavigator.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

[ObservableObject]
public partial class FSNFileSystemTreeViewModel
{
    private FSNMainViewModel fSNMainViewModel;

    [ObservableProperty]
    private FSNDirectoryInfo currentTreeItem;

    [ObservableProperty]
    private IList<FSNDirectoryInfo> dirSource;

    public FSNFileSystemTreeViewModel(FSNMainViewModel fSNMainViewModel)
    {
        this.fSNMainViewModel = fSNMainViewModel;

        FSNDirectoryInfo rootNode = new FSNDirectoryInfo(FSNResource.MyComputer);

        rootNode.Path = FSNResource.MyComputer;

        fSNMainViewModel.CurrentDirectory = rootNode;

        this.DirSource = new List<FSNDirectoryInfo> { rootNode };
    }

    public FSNMainViewModel FSNMainViewModel => this.fSNMainViewModel;

    public void ExpandToCurrentNode(FSNDirectoryInfo fSNDirectoryInfo)
    {
        if (this.CurrentTreeItem != null 
            && (fSNDirectoryInfo.Path.Contains(this.CurrentTreeItem.Path) || this.CurrentTreeItem.Path == FSNResource.MyComputer))
        {
            this.CurrentTreeItem.IsExpanded = false;

            this.CurrentTreeItem.IsExpanded = true;
        }
    }

    partial void OnCurrentTreeItemChanged(FSNDirectoryInfo value)
    {
        fSNMainViewModel.CurrentDirectory = value;
    }
}

// EOF