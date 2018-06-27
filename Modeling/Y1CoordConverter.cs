using System;
using System.Globalization;
using System.Windows.Data;

namespace Modeling
{

    [ValueConversion(typeof(double), typeof(double))]
    public class Y1CoordConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        { 
            double result = 0;
            int count = int.Parse(parameter.ToString());
            for (int i = 0; i <= count; i++)
                result += WApplicationMap.Instance.getRowHeight(i);

            result -= WApplicationMap.Instance.getRowHeight(count) / 2;
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }
}
