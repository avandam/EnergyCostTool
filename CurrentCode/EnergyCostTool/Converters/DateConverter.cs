using System.Globalization;
using System.Windows.Data;

namespace EnergyCostTool.Converters;

public class DateConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return "Unknown";
        }
        DateTime date = (DateTime)value;
        return date.ToString("yyyy-MM-dd");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}