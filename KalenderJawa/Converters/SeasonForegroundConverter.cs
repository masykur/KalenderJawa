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
    public class SeasonForegroundConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Data.SeasonObject seasonObject = value as Data.SeasonObject;
            if (seasonObject != null && seasonObject.IsCurrentSeason)
            {
                return (Brush)Application.Current.Resources["PhoneAccentBrush"];
            }
            else
            {
                return (Brush)Application.Current.Resources["PhoneSubtleBrush"];
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
