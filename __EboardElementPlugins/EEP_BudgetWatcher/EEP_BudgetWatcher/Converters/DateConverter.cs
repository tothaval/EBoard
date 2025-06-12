using System.Globalization;
using System.Windows.Data;

namespace EEP_BudgetWatcher.Converters;


class DateConverter : IValueConverter
{

    // Methods
    #region Methods

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value != null)
        {
            DateTime myDate = (DateTime)value;
            if (myDate != DateTime.MinValue)
            {
                return myDate.ToString("yyyy|MM|dd||HH|mm|ss");
            }
        }

        return String.Empty;
    }


    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    #endregion


}
// EOF