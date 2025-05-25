using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EEP_BudgetWatcher.Components
{
    /// <summary>
    /// Interaktionslogik für BudgetResultMask.xaml
    /// </summary>
    public partial class BudgetResultMask : UserControl
    {

        // Properties & Fields
        #region Properties & Fields


        public double BudgetPerDay
        {
            get { return (double)GetValue(BudgetPerDayProperty); }
            set { SetValue(BudgetPerDayProperty, value); }
        }
        // Using a DependencyProperty as the backing store for NumberOfDays.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BudgetPerDayProperty =
            DependencyProperty.Register("BudgetPerDay", typeof(double), typeof(BudgetResultMask), new PropertyMetadata(0.0));


        public Brush GainExpenseBrush
        {
            get { return (Brush)GetValue(GainExpenseBrushProperty); }
            set { SetValue(GainExpenseBrushProperty, value); }
        }
        // Using a DependencyProperty as the backing store for GainExpenseBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GainExpenseBrushProperty =
            DependencyProperty.Register("GainExpenseBrush", typeof(Brush), typeof(BudgetResultMask), new PropertyMetadata(null));


        public int NumberOfDays
        {
            get { return (int)GetValue(NumberOfDaysProperty); }
            set { SetValue(NumberOfDaysProperty, value); }
        }
        // Using a DependencyProperty as the backing store for NumberOfDays.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NumberOfDaysProperty =
            DependencyProperty.Register("NumberOfDays", typeof(int), typeof(BudgetResultMask), new PropertyMetadata(0));

        public double DaysLeftPercentage
        {
            get { return (double)GetValue(DaysLeftPercentageProperty); }
            set { SetValue(DaysLeftPercentageProperty, value); }
        }
        // Using a DependencyProperty as the backing store for NumberOfDays.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DaysLeftPercentageProperty =
            DependencyProperty.Register("DaysLeftPercentage", typeof(double), typeof(BudgetResultMask), new PropertyMetadata(0.0));


        public DateTime Begin
        {
            get { return (DateTime)GetValue(BeginProperty); }
            set { SetValue(BeginProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Begin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BeginProperty =
            DependencyProperty.Register("Begin", typeof(DateTime), typeof(BudgetResultMask), new PropertyMetadata(null));


        public DateTime End
        {
            get { return (DateTime)GetValue(EndProperty); }
            set { SetValue(EndProperty, value); }
        }
        // Using a DependencyProperty as the backing store for End.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndProperty =
            DependencyProperty.Register("End", typeof(DateTime), typeof(BudgetResultMask), new PropertyMetadata(null));


        public double InitialBudget
        {
            get { return (double)GetValue(InitialBudgetProperty); }
            set { SetValue(InitialBudgetProperty, value); }
        }
        public static readonly DependencyProperty InitialBudgetProperty =
            DependencyProperty.Register("InitialBudget", typeof(double), typeof(BudgetResultMask), new PropertyMetadata(0.0));

        public double Gains
        {
            get { return (double)GetValue(GainsProperty); }
            set { SetValue(GainsProperty, value); }
        }
        public static readonly DependencyProperty GainsProperty =
            DependencyProperty.Register("Gains", typeof(double), typeof(BudgetResultMask), new PropertyMetadata(0.0));

        public double Expenses
        {
            get { return (double)GetValue(ExpensesProperty); }
            set { SetValue(ExpensesProperty, value); }
        }
        public static readonly DependencyProperty ExpensesProperty =
            DependencyProperty.Register("Expenses", typeof(double), typeof(BudgetResultMask), new PropertyMetadata(0.0));

        public double CurrentBalance
        {
            get { return (double)GetValue(CurrentBalanceProperty); }
            set { SetValue(CurrentBalanceProperty, value); }
        }
        public static readonly DependencyProperty CurrentBalanceProperty =
            DependencyProperty.Register("CurrentBalance", typeof(double), typeof(BudgetResultMask), new PropertyMetadata(0.0));

        #endregion


        // Constructors
        #region Constructors

        public BudgetResultMask()
        {
            InitializeComponent();
        }

        #endregion


    }
}
// EOF