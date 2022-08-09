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
    public class JavaDate :IFormattable
    {
        //private readonly DateTime firstDate = new DateTime(1633, 7, 3);
        private readonly string[] monthNames = new[] { "Suro", "Sapar", "Mulud", "Bakda Mulud", "Jumadil Awal", "Jumadil Akhir", "Rejeb", "Ruwah", "Poso", "Sawal", "Apit", "Besar" };
        //private readonly string[] namaPasaran = new[] { "legi", "paing", "pon", "wage", "kliwon" };

        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        protected internal DayOfWeek DayOfWeek { get; set; }

        public string MonthName 
        {
            get { return monthNames[Month - 1]; }
        }
        protected internal Pasaran Pasaran { get; set; }
        //public string PasaranName
        //{
        //    get 
        //    {
        //        return namaPasaran[(int)this.Pasaran]; 
        //    }
        //}
        public Season Season { get; set; }
        public int DayOfSeason { get; set; }

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
                case "d pppp":
                    returnValue = string.Format(formatProvider, "{0} {1}", Day, Pasaran);
                    break;
                case "dddd, pppp":
                    var dayName = System.Globalization.DateTimeFormatInfo.CurrentInfo.DayNames[(int)DayOfWeek];
                    returnValue = string.Format(formatProvider, "{0}, {1}", dayName, Pasaran);
                    break;
                case "e SSSS":
                    returnValue = string.Format(formatProvider, "{0} {1}", DayOfSeason, Season);
                    break;
                case "e ssss":
                    returnValue = string.Format(formatProvider, "{0} {1}", DayOfSeason, Season.ToString().ToLower(CultureInfo.CurrentCulture));
                    break;
                default:
                    returnValue = string.Format(formatProvider, "{0:d2}/{1:d2}/{2:d4}", Month, Day, Year);
                    break;
            }
            return returnValue;
        }
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{0:d2}/{1:d2}/{2:d4}", Month, Day, Year);
        }
    }
}
