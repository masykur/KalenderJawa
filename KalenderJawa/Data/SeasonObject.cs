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

namespace KalenderJawa.Data
{
    public class SeasonObject
    {
        public Season Season { get; set; }
        public bool IsCurrentSeason { get; set; }
        public string SeasonSchedule { get; set; }
        public SeasonObject(Season season)
        {
            this.Season = season;
        }
    }
}
