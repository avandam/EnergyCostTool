using System.Globalization;
using System.Windows;
using System.Windows.Data;
// ReSharper disable NullnessAnnotationConflictWithJetBrainsAnnotations

namespace EnergyCostTool.Converters;

public class MonthConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int month = (int)value;
        switch (month)
        {
            case 1:
                return "Januari";
            case 2:
                return "Februari";
            case 3:
                return "Maart";
            case 4:
                return "April";
            case 5:
                return "Mei";
            case 6:
                return "Juni";
            case 7:
                return "Juli";
            case 8:
                return "Augustus";
            case 9:
                return "September";
            case 10:
                return "Oktober";
            case 11:
                return "November";
            case 12:
                return "December";
            default:
                return "N/A";
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        //string strValue = value as string;
        //DateTime resultDateTime;
        //if (DateTime.TryParse(strValue, out resultDateTime))
        //{
        //    return resultDateTime;
        //}
        return DependencyProperty.UnsetValue;
    }
}