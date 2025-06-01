using EEP_BudgetWatcher.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EEP_BudgetWatcher.Components
{
    /// <summary>
    /// Interaktionslogik für BudgetItemInputMask.xaml
    /// </summary>
    public partial class BudgetItemInputMask : UserControl
    {

        // Properties & Fields
        #region Properties & Fields

        public Brush GainExpenseBrush
        {
            get { return (Brush)GetValue(GainExpenseBrushProperty); }
            set { SetValue(GainExpenseBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GainExpenseBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GainExpenseBrushProperty =
            DependencyProperty.Register("GainExpenseBrush", typeof(Brush), typeof(BudgetItemInputMask), new PropertyMetadata(null));


        public DateTime BudgetPeriodBegin
        {
            get { return (DateTime)GetValue(BudgetPeriodBeginProperty); }
            set { SetValue(BudgetPeriodBeginProperty, value); }
        }
        public static readonly DependencyProperty BudgetPeriodBeginProperty =
            DependencyProperty.Register("BudgetPeriodBegin", typeof(DateTime), typeof(BudgetItemInputMask), new PropertyMetadata(null));


        public DateTime BudgetPeriodEnd
        {
            get { return (DateTime)GetValue(BudgetPeriodEndProperty); }
            set { SetValue(BudgetPeriodEndProperty, value); }
        }
        public static readonly DependencyProperty BudgetPeriodEndProperty =
            DependencyProperty.Register("BudgetPeriodEnd", typeof(DateTime), typeof(BudgetItemInputMask), new PropertyMetadata(null));


        public BudgetTypes BudgetType
        {
            get { return (BudgetTypes)GetValue(BudgetTypeProperty); }
            set { SetValue(BudgetTypeProperty, value); }
        }
        public static readonly DependencyProperty BudgetTypeProperty =
            DependencyProperty.Register("BudgetType", typeof(BudgetTypes), typeof(BudgetItemInputMask), new PropertyMetadata(BudgetTypes.Expense));


        public BudgetIntervals BudgetInterval
        {
            get { return (BudgetIntervals)GetValue(BudgetIntervalProperty); }
            set { SetValue(BudgetIntervalProperty, value); }
        }
        public static readonly DependencyProperty BudgetIntervalProperty =
            DependencyProperty.Register("BudgetInterval", typeof(BudgetIntervals), typeof(BudgetItemInputMask), new PropertyMetadata(BudgetIntervals.Once));


        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }
        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(BudgetItemInputMask), new PropertyMetadata(null));


        public string Item
        {
            get { return (string)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }
        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register("Item", typeof(string), typeof(BudgetItemInputMask), new PropertyMetadata("description"));


        public int Quantity
        {
            get { return (int)GetValue(QuantityProperty); }
            set { SetValue(QuantityProperty, value); }
        }
        public static readonly DependencyProperty QuantityProperty =
            DependencyProperty.Register("Quantity", typeof(int), typeof(BudgetItemInputMask), new PropertyMetadata(1));


        public double Sum
        {
            get { return (double)GetValue(SumProperty); }
            set { SetValue(SumProperty, value); }
        }
        public static readonly DependencyProperty SumProperty =
            DependencyProperty.Register("Sum", typeof(double), typeof(BudgetItemInputMask), new PropertyMetadata(0.0));


        public double Result
        {
            get { return (double)GetValue(ResultProperty); }
            set { SetValue(ResultProperty, value); }
        }
        public static readonly DependencyProperty ResultProperty =
            DependencyProperty.Register("Result", typeof(double), typeof(BudgetItemInputMask), new PropertyMetadata(0.0));

        #endregion


        // Constructors
        #region Constructors

        public BudgetItemInputMask()
        {
            InitializeComponent();
        }

        #endregion


    }
}
// EOF