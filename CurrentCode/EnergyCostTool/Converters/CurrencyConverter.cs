using System.Globalization;
using System.Windows.Data;

namespace EnergyCostTool.Converters;

public class CurrencyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return "N/A";
        }
        double currency = (double)value;
        return Math.Round(currency, 2, MidpointRounding.AwayFromZero).ToString(CultureInfo.CurrentCulture);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
