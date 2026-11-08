using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EnergyCostTool.Converters;

public class MonthConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return "N/A";
        }
        int month = (int)value;
        return month switch
        {
            1 => "Januari",
            2 => "Februari",
            3 => "Maart",
            4 => "April",
            5 => "Mei",
            6 => "Juni",
            7 => "Juli",
            8 => "Augustus",
            9 => "September",
            10 => "Oktober",
            11 => "November",
            12 => "December",
            _ => "N/A"
        };
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