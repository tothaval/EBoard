using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace EBoard.Utilities;

public class VisibilityConverterToCollapsed : IValueConverter
{

    // methods
    #region methods

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is bool boolValue && boolValue ? Visibility.Visible : Visibility.Collapsed;
    }


    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    #endregion methods


}
// EOF