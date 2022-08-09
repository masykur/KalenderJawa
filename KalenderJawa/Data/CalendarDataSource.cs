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
using Microsoft.Phone.Controls.Primitives;

namespace KalenderJawa.Data
{
    public class CalendarDataSource : ILoopingSelectorDataSource
    {
        public CalendarDataSource()
        {
            this.selectedItem = new Calendar(DateTime.Today);
        }

        public object GetNext(object relativeTo)
        {
            return new Calendar(((Calendar)relativeTo).GregorianDate.AddDays(1));
        }

        public object GetPrevious(object relativeTo)
        {
            return new Calendar(((Calendar)relativeTo).GregorianDate.AddDays(-1));
        }

        private object selectedItem;
        public object SelectedItem
        {
            get
            {
                return this.selectedItem;
            }
            set
            {
                // this will use the Equals method if it is overridden for the data source item class 
                if (!object.Equals(this.selectedItem, value))
                {
                    // save the previously selected item so that we can use it  
                    // to construct the event arguments for the SelectionChanged event 
                    object previousSelectedItem = this.selectedItem;
                    this.selectedItem = value;
                    // fire the SelectionChanged event 
                    this.OnSelectionChanged(previousSelectedItem, this.selectedItem);
                }
            }
        }

        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

        protected virtual void OnSelectionChanged(object oldSelectedItem, object newSelectedItem)
        {
            EventHandler<SelectionChangedEventArgs> handler = this.SelectionChanged;
            if (handler != null)
            {
                handler(this, new SelectionChangedEventArgs(new object[] { oldSelectedItem }, new object[] { newSelectedItem }));
            }
        }
    }
}
