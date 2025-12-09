namespace EEP_FSNavigator.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;

[ObservableObject]
public partial class FSNDirectoryDetailsViewModel
{
    private FSNMainViewModel fSNMainViewModel;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DirectoryInfos))]
    private FSNDirectoryInfo currentItem;

    public IList<FSNDirectoryInfo> DirectoryInfos => this.fSNMainViewModel.CurrentItems;

    public FSNDirectoryDetailsViewModel(FSNMainViewModel fSNMainViewModel)
    {
        this.fSNMainViewModel = fSNMainViewModel;
    }

    public void OpenCurrentObject()
    {
        int objType = this.CurrentItem.DirType;

        if ((ObjectType)this.CurrentItem.DirType == ObjectType.File)
        {
            System.Diagnostics.Process.Start(this.CurrentItem.Path);
        }
        else
        {
            this.fSNMainViewModel.CurrentDirectory = this.CurrentItem;
            this.fSNMainViewModel.FileSystemTreeVM.ExpandToCurrentNode(this.fSNMainViewModel.CurrentDirectory);
        }
    }
}
