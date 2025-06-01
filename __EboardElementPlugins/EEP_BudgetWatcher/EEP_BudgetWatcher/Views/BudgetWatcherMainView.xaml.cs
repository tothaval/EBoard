using System.Windows;
using System.Windows.Controls;

namespace EEP_BudgetWatcher.Views
{
    /// <summary>
    /// Interaktionslogik für BudgetWatcherMainView.xaml
    /// </summary>
    public partial class BudgetWatcherMainView : UserControl
    {
        public BudgetWatcherMainView()
        {
            InitializeComponent();

            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }
    }
}
