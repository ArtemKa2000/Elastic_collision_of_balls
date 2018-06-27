using System;
using System.Globalization;
using System.Windows.Data;

namespace Modeling
{
    [ValueConversion(typeof(double), typeof(double))]
    class X2CoordConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return WApplicationMap.Instance.getColumWidth(int.Parse(parameter.ToString())) / 2+3.8;//+3.8 - підібранє значення для стиковки ліній
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
