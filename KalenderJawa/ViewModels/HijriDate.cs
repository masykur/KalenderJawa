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
    public class HijriDate :IFormattable
    {
        private string[] monthNames;// = new[] { "Muharram", "Safar", "Rabi I", "Rabi II", "Jumada I", "Jumada II", "Rajab", "Sha'ban", "Ramadan", "Shawwal", "Dhu al-Qa'da", "Dhu al-Hijja", "" };
        public HijriDate()
        {
            var currentCulture = System.Globalization.CultureInfo.CurrentCulture;
            if (currentCulture.TwoLetterISOLanguageName == "id")
            {
                monthNames = new[] { "Muharam", "Safar", "Rabiul Awal", "Rabiul Akhir", "Jumadil Awal", "Jumadil Akhir", "Rajab", "Sha'ban", "Ramadhan", "Syawal", "Dhul Qa'dah", "Dhul Hijjah", "" };
            }
            else
            {
                monthNames = new[] { "Muharram", "Safar", "Rabi Awwal", "Rabi Thani", "Jumada Awwal", "Jumada Akhir", "Rajab", "Sha'ban", "Ramadan", "Shawwal", "Dhu al-Qa'da", "Dhu al-Hijja", "" };
            }
        }

        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        public string MonthName 
        {
            get { return monthNames[Month - 1]; }
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            string returnValue = "";
            switch (format)
            {
                case "dd/MM/yyyy":
                    returnValue = string.Format(formatProvider, "{0:d2}/{1:d2}/{2:d4}", Day, Month, Year);
                    break;
                case "MM/dd/yyyy":
                    returnValue = string.Format(formatProvider, "{0:d2}/{1:d2}/{2:d4}", Month, Day, Year);
                    break;
                case "d MMMM yyyy":
                    returnValue = string.Format(formatProvider, "{0:d} {1} {2:d4}", Day, MonthName, Year);
                    break;
                case "dd MMMM yyyy":
                    returnValue = string.Format(formatProvider, "{0:d2} {1} {2:d4}", Day, MonthName, Year);
                    break;
                case "MMMM yyyy":
                    returnValue = string.Format(formatProvider, "{0} {1:d4}", MonthName, Year);
                    break;
                default:
                    returnValue = string.Format(formatProvider, "{0:d2}/{1:d2}/{2:d4}", Month, Day, Year);
                    break;
            }
            return returnValue;
        }
    }
}
