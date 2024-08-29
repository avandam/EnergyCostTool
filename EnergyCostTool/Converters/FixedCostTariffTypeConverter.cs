using EnergyCostTool.Models.Enumerations;
using System.Globalization;
using System.Windows.Data;

namespace EnergyCostTool.Converters;

public class FixedCostTariffTypeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return "Unknown";
        }
        FixedCostTariffType fixedCostTariffType = (FixedCostTariffType)value;
        return fixedCostTariffType.GetDescription();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}