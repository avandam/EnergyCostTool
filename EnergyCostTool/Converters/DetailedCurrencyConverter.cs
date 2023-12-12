using System.Globalization;
using System.Windows.Data;

namespace EnergyCostTool.Converters;

public class DetailedCurrencyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return "N/A";
        }
        double currency = (double)value;
        return currency.ToString(CultureInfo.CurrentCulture);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
