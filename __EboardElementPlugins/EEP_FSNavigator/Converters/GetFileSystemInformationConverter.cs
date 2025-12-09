namespace EEP_FSNavigator.Converters;

using EEP_FSNavigator.Models;
using EEP_FSNavigator.ViewModels;
using Serilog;
using System;
using System.Linq;
using System.Windows.Data;

public class GetFileSystemInformationConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        try
        {
            if (value is not FSNDirectoryInfo nodeToExpand)
            {
                return null!;
            }

            // return the subdirectories of the Current Node
            if ((ObjectType)nodeToExpand.DirType == ObjectType.MyComputer)
            {
                return (from sd in FSNScanService.GetRootDirectories()
                        select new FSNDirectoryInfo(sd)).ToList();
            }
            else
            {
                return (from dirs in FSNScanService.GetChildDirectories(nodeToExpand.Path)
                        select new FSNDirectoryInfo(dirs)).ToList();
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);

            return null;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

// EOF