using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Modeling
{
    [ValueConversion(typeof(Thickness), typeof(Thickness))]
    class Y3CoordConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            String[] dat = value.ToString().Split(',');
            String[] matrix = parameter.ToString().Split('_');
            Thickness result = new Thickness();
            if (matrix[0].CompareTo("0") != 0)
                result.Left = int.Parse(dat[0]) * -1;
            if (matrix[1].CompareTo("0") != 0)
                result.Top = int.Parse(dat[1]) * -1;
            if (matrix[2].CompareTo("0") != 0)
                result.Right = int.Parse(dat[2])*-1;
            if (matrix[3].CompareTo("0") != 0)
                result.Bottom = int.Parse(dat[3]) * -1;
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }
}
