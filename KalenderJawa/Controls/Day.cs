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

namespace KalenderJawa.Controls
{
    public class Day : System.Windows.Controls.Button
    {
        public Day()
            : base()
        {
            DefaultStyleKey = typeof(Day);
        }
        private static JavaCalendar javaCalendar = new JavaCalendar();
        private static HijriCalendar hijriCalendar = new HijriCalendar();
        public static readonly DependencyProperty DateProperty = 
            DependencyProperty.Register("Date", typeof(DateTime), typeof(Day), new PropertyMetadata(DateTime.Today, OnValueChanged));
        public static readonly DependencyProperty JavaDateProperty =
            DependencyProperty.Register("JavaDate", typeof(JavaDate), typeof(Day), new PropertyMetadata(javaCalendar.GetJavaDate(DateTime.Today), OnValueChanged));
        public static readonly DependencyProperty HijriDateProperty =
            DependencyProperty.Register("HijriDate", typeof(HijriDate), typeof(Day), new PropertyMetadata(new HijriDate { Year = hijriCalendar.GetYear(DateTime.Today), Month = hijriCalendar.GetMonth(DateTime.Today), Day = hijriCalendar.GetDayOfMonth(DateTime.Today) }, OnValueChanged));
        public static readonly DependencyProperty PasaranProperty =
            DependencyProperty.Register("Pasaran", typeof(Pasaran), typeof(Day), new PropertyMetadata(Pasaran.Legi, OnPasaranChanged));
        public static readonly DependencyProperty PasaranLowerStringProperty =
            DependencyProperty.Register("PasaranLowerString", typeof(string), typeof(Day), new PropertyMetadata(Pasaran.Legi.ToString().ToLower(CultureInfo.CurrentCulture), OnPasaranChanged));
        //public static readonly new DependencyProperty BackgroundProperty =
        //    DependencyProperty.Register("Background", typeof(Brush), typeof(Day), new PropertyMetadata(null, OnValueChanged));
        
        public DateTime Date {
            get
            {
                return (DateTime)base.GetValue(DateProperty);
            }
            set
            {
                base.SetValue(DateProperty, value);
                UpdateDate(value);
            }
        }
        public JavaDate JavaDate
        {
            get
            {
                return (JavaDate)base.GetValue(JavaDateProperty);
            }
            set
            {
                base.SetValue(JavaDateProperty, value);
            }
        }
        public HijriDate HijriDate
        {
            get
            {
                return (HijriDate)base.GetValue(HijriDateProperty);
            }
            set
            {
                base.SetValue(HijriDateProperty, value);
            }
        }
        private void UpdateDate(DateTime date)
        {
            var javaDate = javaCalendar.GetJavaDate(date);
            this.JavaDate = javaDate;
            this.Pasaran = javaDate.Pasaran;
            this.PasaranLowerString = javaDate.Pasaran.ToString().ToLower(CultureInfo.CurrentCulture);
            this.HijriDate = new HijriDate { Year = hijriCalendar.GetYear(date), Month = hijriCalendar.GetMonth(date), Day = hijriCalendar.GetDayOfMonth(date) };
        }

        public Pasaran Pasaran
        {
            get
            {
                return (Pasaran) base.GetValue(PasaranProperty);
            }
            private set
            {
                base.SetValue(PasaranProperty, value);
            }
        }

        public string PasaranLowerString
        {
            get
            {
                return (string)base.GetValue(PasaranLowerStringProperty);
            }
            private set
            {
                base.SetValue(PasaranLowerStringProperty, value);
            }
        }

        //public new Brush Background
        //{
        //    get
        //    {
        //        return (Brush)base.GetValue(BackgroundProperty);
        //    }
        //    set
        //    {
        //        base.SetValue(BackgroundProperty, value);
        //    }
        //}

        static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            //UpdateJavaDate((DateTime)args.NewValue);   
            //Day day = obj as Day;
        }
        static void OnPasaranChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            //var t = obj.ToString();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ContentPresenter presenter = this.GetTemplateChild("ContentContainer") as ContentPresenter;
        }
    }
}
