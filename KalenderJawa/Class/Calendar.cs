using System;
using System.ComponentModel;

namespace KalenderJawa
{
    public class Calendar : INotifyPropertyChanged
    {
        public Calendar()
        {
            this.GregorianDate = DateTime.Today;
        }
        public Calendar(DateTime gregorianDate)
        {
            this.GregorianDate = gregorianDate;
        }
        private DateTime gregorianDate;
        public DateTime GregorianDate 
        {
            get { return this.gregorianDate; }
            set { 
                this.gregorianDate = value;
                System.Globalization.HijriCalendar hijriCalendar = new System.Globalization.HijriCalendar();
                this.HijriDate = new HijriDate { Day = hijriCalendar.GetDayOfMonth(GregorianDate), Month = hijriCalendar.GetMonth(GregorianDate), Year = hijriCalendar.GetYear(GregorianDate) };
                JavaCalendar javaCalendar = new JavaCalendar();
                this.JavaDate = javaCalendar.GetJavaDate(GregorianDate);
                NotifyPropertyChanged("GregorianDate");
                NotifyPropertyChanged("HijriDate");
                NotifyPropertyChanged("JavaDate");
            } 
        }
        public HijriDate HijriDate { get; private set; }
        public JavaDate JavaDate { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
