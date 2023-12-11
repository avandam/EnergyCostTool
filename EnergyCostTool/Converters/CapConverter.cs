using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace EnergyCostTool.Converters
{
    public class CapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double cap = (double)value;
            if (cap == 1000)
            {
                return "N/A";
            }
            return cap.ToString();
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
}
