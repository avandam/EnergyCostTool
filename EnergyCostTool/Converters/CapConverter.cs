using System.Globalization;
using System.Windows.Data;

namespace EnergyCostTool.Converters;

public class CapConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        double cap = (double)value;
        if (Math.Abs(cap - 1000) < 1)
        {
            return "N/A";
        }
        return cap.ToString(CultureInfo.InvariantCulture);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
