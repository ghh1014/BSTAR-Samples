using System;
using System.Globalization;
using System.Windows.Data;

namespace AxisMgntSample.Converters
{
    public class AxisTypeToStrCvt:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (AxisType)value;
            return type == AxisType.VerticalAxis ? "纵轴" : "横轴";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
