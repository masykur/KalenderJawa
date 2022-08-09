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
using System.Globalization;

namespace KalenderJawa
{
    public class Month
    {
        public int MonthNumber { get; set; }
        public string MonthName
        {
            get
            {
                var culture = CultureInfo.CurrentCulture;
                return culture.DateTimeFormat.MonthNames[MonthNumber - 1].ToLower(culture);
            }
        }
    }
}
