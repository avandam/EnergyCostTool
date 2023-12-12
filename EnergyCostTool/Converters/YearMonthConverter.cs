using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace EnergyCostTool.Converters;

public class YearMonthConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return "N/A";
        }
        DateTime date = (DateTime)value;
        string result = date.Month switch
        {
            1 => "Jan",
            2 => "Feb",
            3 => "Maa",
            4 => "Apr",
            5 => "Mei",
            6 => "Jun",
            7 => "Jul",
            8 => "Aug",
            9 => "Sep",
            10 => "Okt",
            11 => "Nov",
            12 => "Dec",
            _ => "N/A"
        };

        return result + " " + date.Year;
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