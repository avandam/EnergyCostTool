using System.Globalization;
using System.Windows.Data;
using EnergyCostTool.Data;

namespace EnergyCostTool.Converters;

public class FixedCostTypeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        FixedCostType fixedCostType = (FixedCostType)value;
        return fixedCostType.GetDescription();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}