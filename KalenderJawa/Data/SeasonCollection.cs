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
using System.Collections.ObjectModel;
using System.Globalization;

namespace KalenderJawa.Data
{
    public class SeasonCollection : ObservableCollection<SeasonObject>
    {
        public SeasonCollection()
            : base()
        {
            DateTime[] seasionsBegin = new[] 
            { 
                new DateTime(DateTime.Today.Year, 6, 23), 
                new DateTime(DateTime.Today.Year, 8, 3), 
                new DateTime(DateTime.Today.Year, 8, 25), 
                new DateTime(DateTime.Today.Year, 9, 19), 
                new DateTime(DateTime.Today.Year, 10, 14), 
                new DateTime(DateTime.Today.Year, 11, 10), 
                new DateTime(DateTime.Today.Year, 12, 23), 
                new DateTime(DateTime.Today.Year, 2, 4), 
                new DateTime(DateTime.Today.Year, 3, 2), 
                new DateTime(DateTime.Today.Year, 3, 27), 
                new DateTime(DateTime.Today.Year, 4, 20), 
                new DateTime(DateTime.Today.Year, 5, 13)
            };
            DateTime[] seasionsEnd = new[] 
            { 
                new DateTime(DateTime.Today.Year, 8, 2), 
                new DateTime(DateTime.Today.Year, 8, 24), 
                new DateTime(DateTime.Today.Year, 9, 18), 
                new DateTime(DateTime.Today.Year, 10, 13), 
                new DateTime(DateTime.Today.Year, 11, 9), 
                new DateTime(DateTime.Today.Year, 12, 22), 
                new DateTime(DateTime.Today.Year, 2, 3), 
                new DateTime(DateTime.Today.Year, 3, 1), 
                new DateTime(DateTime.Today.Year, 3, 26), 
                new DateTime(DateTime.Today.Year, 4, 19), 
                new DateTime(DateTime.Today.Year, 5, 12), 
                new DateTime(DateTime.Today.Year, 6, 22)
            };
            var calendar = new Calendar(DateTime.Today);
            for (int i = 0; i < 12; i++)
            {
                var season = new SeasonObject((Season)i);
                season.IsCurrentSeason = season.Season == calendar.JavaDate.Season;
                season.SeasonSchedule = string.Format(CultureInfo.CurrentCulture, "{0:d MMMM} - {1:d MMMM}", seasionsBegin[i], seasionsEnd[i]);
                Add(season);
            }
        }

    }
}
