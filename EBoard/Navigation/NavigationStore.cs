/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  NavigationStore 
 * 
 *  helper class for ViewModel changes
 */
using CommunityToolkit.Mvvm.ComponentModel;

namespace EBoard.Navigation;

public class NavigationStore
{
    private ObservableObject _baseViewModel;

    public ObservableObject CurrentViewModel
    {
        get { return _baseViewModel; }
        set
        {
            _baseViewModel = value;
            OnCurrentViewModelChanged();
        }
    }

    public event Action CurrentViewModelChanged;

    private void OnCurrentViewModelChanged()
    {
        CurrentViewModelChanged?.Invoke();
    }

}
// EOF