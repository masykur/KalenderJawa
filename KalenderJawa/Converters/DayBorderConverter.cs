using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;

namespace KalenderJawa
{
    public class DayBorderConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Calendar calendar = value as Calendar;
            if (calendar != null && calendar.GregorianDate.Date == DateTime.Today)
            {
                return (Brush)Application.Current.Resources["PhoneAccentBrush"];
            }
            else
            {
                return new SolidColorBrush(Colors.Transparent);// (Brush)Application.Current.Resources["PhoneBackgroundBrush"];
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
