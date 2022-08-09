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
using System.Globalization;

namespace KalenderJawa
{
    public class LocalizeDateLowerCaseConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DateTime? date = value as DateTime?;
            if (date.HasValue)
            {
                if (parameter != null)
                {
                    return date.Value.ToString(parameter.ToString(), CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture);
                }
                else
                {
                    return date.Value.ToLongDateString().ToLower(CultureInfo.CurrentCulture);
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
