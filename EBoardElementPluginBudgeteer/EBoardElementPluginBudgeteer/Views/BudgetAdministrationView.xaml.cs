using EBoardElementPluginBudgeteer.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace EBoardElementPluginBudgeteer.Views
{
    /// <summary>
    /// Interaktionslogik für BudgetAdministrationView.xaml
    /// </summary>
    public partial class BudgetAdministrationView : UserControl
    {



        public BudgetViewModel BudgetVM
        {
            get { return (BudgetViewModel)GetValue(BudgetVMProperty); }
            set { SetValue(BudgetVMProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BudgetVM.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BudgetVMProperty =
            DependencyProperty.Register("BudgetVM", typeof(BudgetViewModel), typeof(BudgetAdministrationView), new PropertyMetadata(null));




        public BudgetAdministrationView()
        {
            InitializeComponent();
        }
    }
}
