using System.Windows;
using System.Windows.Controls;

namespace EBoardElementPluginBudgeteer.UserControls
{
    /// <summary>
    /// Interaktionslogik für BudgetSetup.xaml
    /// </summary>
    public partial class BudgetSetup : UserControl
    {
        public BudgetSetup()
        {
            InitializeComponent();
        }


        public bool IsBudgetSetup
        {
            get { return (bool)GetValue(IsBudgetSetupProperty); }
            set { SetValue(IsBudgetSetupProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsBudgetSetup.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsBudgetSetupProperty =
            DependencyProperty.Register("IsBudgetSetup", typeof(bool), typeof(BudgetSetup), new PropertyMetadata(false));


        public DateTime BudgetBeginDate
        {
            get { return (DateTime)GetValue(BudgetBeginDateProperty); }
            set { SetValue(BudgetBeginDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BudgetBeginDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BudgetBeginDateProperty =
            DependencyProperty.Register("BudgetBeginDate", typeof(DateTime), typeof(BudgetSetup), new PropertyMetadata(DateTime.Now));


        public DateTime BudgetEndDate
        {
            get { return (DateTime)GetValue(BudgetEndDateProperty); }
            set { SetValue(BudgetEndDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BudgetEndDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BudgetEndDateProperty =
            DependencyProperty.Register("BudgetEndDate", typeof(DateTime), typeof(BudgetSetup), new PropertyMetadata(DateTime.Now));


        public string BudgetName
        {
            get { return (string)GetValue(BudgetNameProperty); }
            set { SetValue(BudgetNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BudgetName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BudgetNameProperty =
            DependencyProperty.Register("BudgetName", typeof(string), typeof(BudgetSetup), new PropertyMetadata(string.Empty));


        public decimal BudgetInitialSum
        {
            get { return (decimal)GetValue(BudgetInitialSumProperty); }
            set { SetValue(BudgetInitialSumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BudgetInitialSum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BudgetInitialSumProperty =
            DependencyProperty.Register("BudgetInitialSum", typeof(decimal), typeof(BudgetSetup), new PropertyMetadata(0.0m));





    }
}
