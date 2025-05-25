using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml.Serialization;

namespace EEP_BudgetWatcher.Resources
{
    [Serializable]
    [XmlRoot("ResourceSet")]
    public class ResourceSet
    { 
        
        // properties & fields
        #region properties

        public Color C_Background { get; set; }


        public Color C_Selection { get; set; }


        public Color C_Text { get; set; }


        public Color C_Expense { get; set; }


        public Color C_Gain { get; set; }


        public Color C_Text_Header { get; set; }


        public CornerRadius ButtonCornerRadius { get; set; }

        public CornerRadius VisibilityFieldCornerRadius { get; set; }



        [XmlIgnore]
        public FontFamily FF { get; set; } = new FontFamily("Verdana");


        public string FontFamily { get; set; } = "Verdana";


        /// <summary>
        /// FontSize
        /// </summary>
        public double FS { get; set; } = 11.0;


        /// <summary>
        /// HeaderFontsize
        /// </summary>
        public double HFS { get; set; } = 11.0;


        public string Language { get; set; } = "English";


        [XmlIgnore]
        public CultureInfo CurrentCulture { get; set; } = CultureInfo.CurrentCulture;


        [XmlIgnore]
        public SolidColorBrush BackgroundBrush { get; set; } = new SolidColorBrush(Colors.White);


        [XmlIgnore]
        public SolidColorBrush SelectionBrush { get; set; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AEF"));


        [XmlIgnore]
        public SolidColorBrush TextBrush { get; set; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#444"));


        [XmlIgnore]
        public SolidColorBrush HeaderBrush { get; set; } = new SolidColorBrush(Colors.Black);

        [XmlIgnore]
        public SolidColorBrush GainBrush { get; set; } = new SolidColorBrush(Colors.Green);


        [XmlIgnore]
        public SolidColorBrush ExpenseBrush { get; set; } = new SolidColorBrush(Colors.Red);

        #endregion properties


        // methods
        #region methods

        public ResourceSet GetResources()
        {
            // sprachdatei.xml laden und werte zuweisen?
            // liste aus gefundenen sprachdateien einlesen und in combobox itemsource reinladen
            // statt des enums? enums lassen sich glaub nicht erweitern.
            Language = Application.Current.Resources["Language"].ToString();


            BackgroundBrush = (SolidColorBrush)Application.Current.Resources["BackgroundBrush"];

            C_Background = BackgroundBrush.Color;


            SelectionBrush = (SolidColorBrush)Application.Current.Resources["SelectionBrush"];

            C_Selection = SelectionBrush.Color;


            TextBrush = (SolidColorBrush)Application.Current.Resources["TextBrush"];

            C_Text = TextBrush.Color;



            GainBrush = (SolidColorBrush)Application.Current.Resources["GainBrush"];

            C_Gain = GainBrush.Color;



            ExpenseBrush = (SolidColorBrush)Application.Current.Resources["ExpenseBrush"];

            C_Expense = ExpenseBrush.Color;


            HeaderBrush = (SolidColorBrush)Application.Current.Resources["HeaderBrush"];

            C_Text_Header = HeaderBrush.Color;



            FF = (FontFamily)Application.Current.Resources["FF"];

            FontFamily = FF.Source;


            FS = (double)Application.Current.Resources["FS"];


            HFS = (double)Application.Current.Resources["FS"] * 1.5;


            ButtonCornerRadius = (CornerRadius)Application.Current.Resources["Button_CornerRadius"];
            VisibilityFieldCornerRadius = (CornerRadius)Application.Current.Resources["VisibilityField_CornerRadius"];


            return this;
        }


        public void FindLanguages()
        {

        }


        public void SetResources()
        {
            Application.Current.Resources["Language"] = Language;

            Application.Current.Resources["FS"] = FS;
            Application.Current.Resources["FF"] = new FontFamily(FontFamily);

            Application.Current.Resources["HFS"] = FS * 1.25;

            Application.Current.Resources["Button_CornerRadius"] = ButtonCornerRadius;

            Application.Current.Resources["VisibilityField_CornerRadius"] = VisibilityFieldCornerRadius;


            Application.Current.Resources["BackgroundBrush"] = new SolidColorBrush(C_Background);
            Application.Current.Resources["TextBrush"] = new SolidColorBrush(C_Text);
            Application.Current.Resources["HeaderBrush"] = new SolidColorBrush(C_Text_Header);
            Application.Current.Resources["SelectionBrush"] = new SolidColorBrush(C_Selection);
            Application.Current.Resources["GainBrush"] = new SolidColorBrush(C_Gain);
            Application.Current.Resources["ExpenseBrush"] = new SolidColorBrush(C_Expense);

            Application.Current.Resources["Culture"] = XmlLanguage.GetLanguage(CurrentCulture.IetfLanguageTag);
        }

        #endregion methods
    }
}
// EOF