/*  BudgetWatcher (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  SetupFieldViewModel  : BaseViewModel
 * 
 *  viewmodel for SetupField component
 */
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using PropertyTools;


namespace EEP_BudgetWatcher.ViewModels;


public partial class SetupFieldViewModel : ObservableObject
{

    // properties & fields
    #region properties

    [ObservableProperty]
    private Brush _background;


    [ObservableProperty]
    private Color _backgroundColor;

    partial void OnBackgroundColorChanged(Color value)
    {
        Background = new SolidColorBrush(BackgroundColor);
        Application.Current.Resources["BackgroundBrush"] = Background;
    }


    [ObservableProperty]
    private double _ButtonCornerRadius;

    partial void OnButtonCornerRadiusChanged(double value)
    {
        Application.Current.Resources["Button_CornerRadius"] = new CornerRadius(_ButtonCornerRadius);
    }


    [ObservableProperty]
    private Brush _ExpenseBrush;


    [ObservableProperty]
    private Color _ExpenseColor;

    partial void OnExpenseColorChanged(Color value)
    {
        ExpenseBrush = new SolidColorBrush(ExpenseColor);
        Application.Current.Resources["ExpenseBrush"] = ExpenseBrush;

        GainExpenseColorChange?.Invoke(this, EventArgs.Empty);
    }

    [ObservableProperty]
    private FontFamily _font;

    partial void OnFontChanged(FontFamily value)
    {
        Application.Current.Resources["FF"] = Font;
    }


    [ObservableProperty]
    private double _fontSize;


    [ObservableProperty]
    private Brush _foreground;


    [ObservableProperty]
    private Color _foregroundColor;

    partial void OnForegroundColorChanged(Color value)
    {
        Foreground = new SolidColorBrush(ForegroundColor);
        Application.Current.Resources["TextBrush"] = Foreground;
    }


    [ObservableProperty]
    private Brush _GainBrush;


    [ObservableProperty]
    private Color _GainColor;

    partial void OnGainColorChanged(Color value)
    {
        GainBrush = new SolidColorBrush(GainColor);

        Application.Current.Resources["GainBrush"] = GainBrush;
        GainExpenseColorChange?.Invoke(this, EventArgs.Empty);
    }


    [ObservableProperty]
    private Brush _headerText;


    [ObservableProperty]
    private Color _headerTextColor;

    partial void OnHeaderTextColorChanged(Color value)
    {
        HeaderText = new SolidColorBrush(HeaderTextColor);
        Application.Current.Resources["HeaderBrush"] = HeaderText;
    }


    [ObservableProperty]
    private string _Language;

    partial void OnLanguageChanged(string value)
    {
        Application.Current.Resources["Language"] = _Language;
    }


    [ObservableProperty]
    private Brush _selection;



    [ObservableProperty]
    private Color _selectionColor;

    partial void OnSelectionColorChanged(Color value)
    {
        Selection = new SolidColorBrush(SelectionColor);
        Application.Current.Resources["SelectionBrush"] = Selection;
    }


    [ObservableProperty]
    private CultureInfo _SelectedCulture;

    partial void OnSelectedCultureChanged(CultureInfo value)
    {
        Application.Current.Resources["Culture"] = XmlLanguage.GetLanguage(_SelectedCulture.IetfLanguageTag);
    }


    [ObservableProperty]
    private string _SelectedLanguage;

    partial void OnSelectedLanguageChanged(string value)
    {
        Language = value;

        //new LanguageResources(_SelectedLanguage);
    }


    [ObservableProperty]
    private double _VisibilityFieldCornerRadius;

    partial void OnVisibilityFieldCornerRadiusChanged(double value)
    {
        Application.Current.Resources["VisibilityField_CornerRadius"] = new CornerRadius(_VisibilityFieldCornerRadius);

        if (_VisibilityFieldCornerRadius < 40)
        {
            Application.Current.Resources["VisibilityFieldBorderPadding"] = new Thickness(10);

            return;
        }

        Application.Current.Resources["VisibilityFieldBorderPadding"] = new Thickness(_VisibilityFieldCornerRadius / 4);
    }

    #endregion properties


    // Event Properties
    #region Event Properties

    public EventHandler GainExpenseColorChange;

    #endregion


    // collections
    #region collections

    private ObservableCollection<CultureInfo> _Currency;
    public ObservableCollection<CultureInfo> Currency
    {
        get { return _Currency; }
        set
        {
            _Currency = value;

            OnPropertyChanged(nameof(Currency));
        }
    }


    private ObservableCollection<string> _Languages;
    public ObservableCollection<string> Languages
    {
        get { return _Languages; }
        set
        {
            _Languages = value;
            OnPropertyChanged(nameof(Languages));
        }
    }

    #endregion collections



    // constructors
    #region constructors

    public SetupFieldViewModel()
    {
        FontSize = (double)Application.Current.Resources["FS"];
        Font = (FontFamily)Application.Current.Resources["FF"];

        BackgroundColor = ((SolidColorBrush)Application.Current.Resources["BackgroundBrush"]).Color;
        ForegroundColor = ((SolidColorBrush)Application.Current.Resources["TextBrush"]).Color;
        HeaderTextColor = ((SolidColorBrush)Application.Current.Resources["HeaderBrush"]).Color;
        SelectionColor = ((SolidColorBrush)Application.Current.Resources["SelectionBrush"]).Color;
        GainColor = ((SolidColorBrush)Application.Current.Resources["GainBrush"]).Color;
        ExpenseColor = ((SolidColorBrush)Application.Current.Resources["ExpenseBrush"]).Color;

        ButtonCornerRadius = ((CornerRadius)Application.Current.Resources["Button_CornerRadius"]).TopLeft;

        VisibilityFieldCornerRadius = ((CornerRadius)Application.Current.Resources["VisibilityField_CornerRadius"]).TopLeft;

        //Languages = new LanguageResources().LoadLanguages();

        OnPropertyChanged(nameof(Languages));


        if (Application.Current.Resources["Language"] != null)
        {
            SelectedLanguage = Application.Current.Resources["Language"].ToString();
        }
        else
        {
            SelectedLanguage = "English.xml";
        }


        Currency = new ObservableCollection<CultureInfo>(CultureInfo.GetCultures(CultureTypes.SpecificCultures).ToList());


    }

    #endregion constructors


    // methods
    #region methods

    [RelayCommand]
    private void ApplyFontSize(object s)
    {
        Application.Current.Resources["FS"] = FontSize;
        Application.Current.Resources["HFS"] = FontSize * 1.25;
    }

    #endregion methods


}
// EOF