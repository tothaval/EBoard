using BudgetManagement.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace EBoardElementPluginBudgeteer.UserControls
{
    /// <summary>
    /// Interaktionslogik für BudgetChangeSetup.xaml
    /// </summary>
    public partial class BudgetChangeSetup : UserControl
    {
        public BudgetChangeSetup()
        {
            InitializeComponent();
        }

        public BudgetChangeType Type
        {
            get { return (BudgetChangeType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Type.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(BudgetChangeType), typeof(BudgetChangeSetup), new PropertyMetadata(BudgetChangeType.Expense));


        public bool CanExecute
        {
            get { return (bool)GetValue(CanExecuteProperty); }
            set { SetValue(CanExecuteProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanExecute.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanExecuteProperty =
            DependencyProperty.Register("CanExecute", typeof(bool), typeof(BudgetChangeSetup), new PropertyMetadata(true));




        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Date.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(BudgetChangeSetup), new PropertyMetadata(DateTime.Now));




        public string Item
        {
            get { return (string)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Item.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register("Item", typeof(string), typeof(BudgetChangeSetup), new PropertyMetadata(string.Empty));




        public decimal Price
        {
            get { return (decimal)GetValue(PriceProperty); }
            set { SetValue(PriceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Price.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PriceProperty =
            DependencyProperty.Register("Price", typeof(decimal), typeof(BudgetChangeSetup), new PropertyMetadata(0.0m));





        public int Quantity
        {
            get { return (int)GetValue(QuantityProperty); }
            set { SetValue(QuantityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Quantity.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty QuantityProperty =
            DependencyProperty.Register("Quantity", typeof(int), typeof(BudgetChangeSetup), new PropertyMetadata(1));




    }
}
