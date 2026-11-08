using EnergyCostTool.Models.Enumerations;
using System.Globalization;
using System.Windows.Data;

namespace EnergyCostTool.Converters;

public class FixedCostTypeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return "Unknown";
        }

        FixedCostType fixedCostType = (FixedCostType)value;
        return fixedCostType.GetDescription();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}