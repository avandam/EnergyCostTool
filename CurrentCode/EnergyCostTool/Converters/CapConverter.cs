using System.Globalization;
using System.Windows.Data;

namespace EnergyCostTool.Converters;

public class CapConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return "N/A";
        }
        double cap = (double)value;
        if (Math.Abs(cap - 1000) < 1)
        {
            return "N/A";
        }
        return "\u20AC " + cap.ToString(CultureInfo.CurrentCulture);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
