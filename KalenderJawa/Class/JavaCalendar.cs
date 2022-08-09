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
    public class JavaCalendar
    {
        private readonly DateTime firstDate = new DateTime(1633, 7, 3);
        private readonly int firstJavaYear = 1555;// new Date { Year = 1555, Month = 1, Day = 1 };
        private readonly Weton[] satuSuro = new[] { 
            new Weton { Hari = Hari.Selasa,  Pasaran = Pasaran.Pon },
            new Weton { Hari = Hari.Sabtu, Pasaran = Pasaran.Paing },
            new Weton { Hari = Hari.Kamis, Pasaran=Pasaran.Paing},
            new Weton { Hari = Hari.Senin , Pasaran=Pasaran.Legi},
            new Weton { Hari = Hari.Jumat, Pasaran=Pasaran.Kliwon},
            new Weton { Hari = Hari.Rabu , Pasaran=Pasaran.Kliwon},
            new Weton { Hari = Hari.Ahad , Pasaran=Pasaran.Wage},
            new Weton { Hari = Hari.Kamis , Pasaran=Pasaran.Pon}
        };

        private readonly int[][] daysAge = new[] 
        {
            new [] {30, 29, 30, 29, 30, 29, 30, 29, 30, 29, 30, 29}, // alip
            new [] {30, 29, 30, 29, 30, 29, 30, 29, 30, 29, 30, 30}, // ehe (kabisat)
            new [] {30, 29, 30, 29, 30, 29, 30, 29, 30, 29, 30, 29}, // jumawal
            new [] {30, 29, 30, 29, 30, 29, 30, 29, 30, 29, 30, 30}, // je (kabisat)
            new [] {30, 30, 30, 29, 29, 29, 30, 29, 30, 29, 30, 29}, // dal
            new [] {30, 29, 30, 29, 30, 29, 30, 29, 30, 29, 30, 29}, // be
            new [] {30, 29, 30, 29, 30, 29, 30, 29, 30, 29, 30, 29}, // wawu
            new [] {30, 29, 30, 29, 30, 29, 30, 29, 30, 29, 30, 30}  // jimakir (kabisat)
        };
        private Weton Weton(DateTime date)
        {
            int hari = (int)date.DayOfWeek;
            var days = (date - firstDate).Days;
            int pasaran = days % 5;
            return new Weton { Hari = (Hari)hari, Pasaran = (Pasaran)pasaran };
        }
        public JavaDate GetJavaDate(DateTime gregorianDate)
        {
            var date = new JavaDate();
            var days = (gregorianDate - firstDate).Days - 1;
            var year = (int)((double)days / 354.375d);

            date.Year = year + firstJavaYear;
            var windu = year % 8;
            var hariSatuSuro = satuSuro[windu];

            var deltaDay = (int)(days - (year * 354.375d));
            var satuSuroDate = gregorianDate.AddDays(-1 * deltaDay);
            var satuSuroDateAdjusment = satuSuroDate;
            int i = 0;
            int d = 0;
            Weton hp = Weton(satuSuroDate);
            while (!hp.Equals(hariSatuSuro))
            {
                i++;
                d = -1;
                hp = Weton(satuSuroDate.AddDays(d * i));
                if (hp.Equals(hariSatuSuro))
                {
                    break;
                }
                d = 1;
                hp = Weton(satuSuroDate.AddDays(i));
            }
            satuSuroDateAdjusment = satuSuroDate.AddDays(d);
            //HijriCalendar hijriCalendar = new HijriCalendar();

            var newYear = satuSuroDateAdjusment;
            var daysLeft = (gregorianDate - newYear).Days + 1;
            var month = 0;
            while (daysLeft > daysAge[windu][month])
            {
                daysLeft -= daysAge[windu][month];
                month++;
            }
            date.Month = month + 1;
            date.Day = daysLeft;
            date.Pasaran = (Pasaran)((gregorianDate - firstDate).Days % 5);

            date.Season = Season.Kapitu;
            DateTime[] beginSeasions = new[] 
            { 
                new DateTime(gregorianDate.Year, 2, 4), 
                new DateTime(gregorianDate.Year, 3, 2), 
                new DateTime(gregorianDate.Year, 3, 27), 
                new DateTime(gregorianDate.Year, 4, 20), 
                new DateTime(gregorianDate.Year, 5, 13), 
                new DateTime(gregorianDate.Year, 6, 23), 
                new DateTime(gregorianDate.Year, 8, 3), 
                new DateTime(gregorianDate.Year, 8, 25), 
                new DateTime(gregorianDate.Year, 9, 19), 
                new DateTime(gregorianDate.Year, 10, 14), 
                new DateTime(gregorianDate.Year, 11, 10), 
                new DateTime(gregorianDate.Year, 12, 23) 
            };
            for(i = 0; i < 12; i++) 
            {
                if (gregorianDate >= beginSeasions[i])
                {
                    date.DayOfSeason = (gregorianDate - beginSeasions[i]).Days + 1;
                    date.Season = (Season)((i + 7) % 12);
                }
                else
                {
                    break;
                }
            }
            if (date.Season == Season.Kapitu && gregorianDate.Month <= 2)
            {
                date.DayOfSeason = (gregorianDate - new DateTime(gregorianDate.Year, 1, 1)).Days + 10;
            }
            date.DayOfWeek = gregorianDate.DayOfWeek;

            //if (gregorianDate >= new DateTime(gregorianDate.Year, 6, 23) && gregorianDate <= new DateTime(gregorianDate.Year, 8, 2))
            //{
            //    date.Season = Season.Kasa;
            //}
            //else if (gregorianDate >= new DateTime(gregorianDate.Year, 8, 3) && gregorianDate <= new DateTime(gregorianDate.Year, 8, 25))
            //{
            //    date.Season = Season.Karo;
            //}
            //else if (gregorianDate >= new DateTime(gregorianDate.Year, 8, 26) && gregorianDate <= new DateTime(gregorianDate.Year, 9, 18))
            //{
            //    date.Season = Season.Katiga;
            //}
            //else if (gregorianDate >= new DateTime(gregorianDate.Year, 9, 19) && gregorianDate <= new DateTime(gregorianDate.Year, 10, 13))
            //{
            //    date.Season = Season.Kapat;
            //}
            //else if (gregorianDate >= new DateTime(gregorianDate.Year, 10, 14) && gregorianDate <= new DateTime(gregorianDate.Year, 11, 9))
            //{
            //    date.Season = Season.Kalima;
            //}
            //else if (gregorianDate >= new DateTime(gregorianDate.Year, 11, 10) && gregorianDate <= new DateTime(gregorianDate.Year, 12, 22))
            //{
            //    date.Season = Season.Kanem;
            //}
            //else if (gregorianDate >= new DateTime(gregorianDate.Year, 12, 23) || gregorianDate <= new DateTime(gregorianDate.Year, 2, 3))
            //{
            //    date.Season = Season.Kapitu;
            //}
            //else if (gregorianDate >= new DateTime(gregorianDate.Year, 2, 4) && gregorianDate <= new DateTime(gregorianDate.Year, 3, 1))
            //{
            //    date.Season = Season.Kawolu;
            //}
            //else if (gregorianDate >= new DateTime(gregorianDate.Year, 3, 1) && gregorianDate <= new DateTime(gregorianDate.Year, 3, 26))
            //{
            //    date.Season = Season.Kasanga;
            //}
            //else if (gregorianDate >= new DateTime(gregorianDate.Year, 3, 27) && gregorianDate <= new DateTime(gregorianDate.Year, 4, 19))
            //{
            //    date.Season = Season.Kadasa;
            //}
            //else if (gregorianDate >= new DateTime(gregorianDate.Year, 4, 20) && gregorianDate <= new DateTime(gregorianDate.Year, 5, 12))
            //{
            //    date.Season = Season.Kawolu;
            //}
            //else if (gregorianDate >= new DateTime(gregorianDate.Year, 5, 13) && gregorianDate <= new DateTime(gregorianDate.Year, 6, 22))
            //{
            //    date.Season = Season.Kawolu;
            //}
            return date;
        }
    }
}
