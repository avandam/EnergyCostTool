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
    public class YearMonthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            string result;
            switch (date.Month)
            {
                case 1:
                    result = "Jan";
                    break;
                case 2:
                    result = "Feb";
                    break;
                case 3:
                    result = "Maa";
                    break;
                case 4:
                    result = "Apr";
                    break;
                case 5:
                    result = "Mei";
                    break;
                case 6:
                    result = "Jun";
                    break;
                case 7:
                    result = "Jul";
                    break;
                case 8:
                    result = "Aug";
                    break;
                case 9:
                    result = "Sep";
                    break;
                case 10:
                    result = "Okt";
                    break;
                case 11:
                    result = "Nov";
                    break;
                case 12:
                    result = "Dec";
                    break;
                default:
                    result = "N/A";
                    break;
            }

            return result + " " + date.Year;
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
