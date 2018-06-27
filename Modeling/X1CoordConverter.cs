using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Modeling
{
    [ValueConversion(typeof(Thickness), typeof(double))]
    class X1CoordConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            String[] dat = value.ToString().Split(',');
            switch (parameter.ToString())
            {
                case "left": return int.Parse(dat[0]) * -1;
                case "top": return int.Parse(dat[1]) * -1;
                case "right": return int.Parse(dat[2]) * -1;
                case "bottom": return int.Parse(dat[3]) * -1;
                default: return 0;
            }
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
