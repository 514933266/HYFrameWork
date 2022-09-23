using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace HYFrameWork.WPF.Converter
{
   public class DoubleConverter : IValueConverter
    {
        public object Convert(object type, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double width = 0.00;
            if (type != null)
            {
                width=(double)type;
            }
            return width;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
