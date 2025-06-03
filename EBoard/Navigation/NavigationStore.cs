/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *
 *  NavigationStore
 *
 *  helper class for ViewModel changes
 */
namespace EBoard.Navigation;

using CommunityToolkit.Mvvm.ComponentModel;

public class NavigationStore
{
    private ObservableObject baseViewModel;

    public event Action CurrentViewModelChanged;

    public ObservableObject CurrentViewModel
    {
        get { return this.baseViewModel; }

        set
        {
            this.baseViewModel = value;
            this.OnCurrentViewModelChanged();
        }
    }

    private void OnCurrentViewModelChanged()
    {
        this.CurrentViewModelChanged?.Invoke();
    }
}

// EOF