using System.Globalization;
using System.Windows.Data;
using EnergyCostTool.Data;

namespace EnergyCostTool.Converters;

public class FixedCostTariffTypeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        FixedCostTariffType fixedCostTariffType = (FixedCostTariffType)value;
        return fixedCostTariffType.GetDescription();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}