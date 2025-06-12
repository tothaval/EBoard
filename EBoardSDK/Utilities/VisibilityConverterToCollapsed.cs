// <copyright file="VisibilityConverterToCollapsed.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK.Utilities;

using System.Globalization;
using System.Windows;
using System.Windows.Data;

public class VisibilityConverterToCollapsed : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is bool boolValue && boolValue ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

// EOF