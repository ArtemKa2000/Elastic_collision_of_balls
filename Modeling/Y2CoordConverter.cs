using System;
using System.Globalization;
using System.Windows.Data;

namespace Modeling
{
    [ValueConversion(typeof(double), typeof(double))]
    class Y2CoordConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return WApplicationMap.Instance.getRowHeight(int.Parse(parameter.ToString()))/2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }
}
