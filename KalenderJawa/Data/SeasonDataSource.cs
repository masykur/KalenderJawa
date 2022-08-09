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
using System.Linq;

namespace KalenderJawa.Data
{
    public class SeasonDataSource : ILoopingSelectorDataSource
    {
        private static readonly SeasonCollection seasonCollection = new SeasonCollection();
        public SeasonDataSource()
        {
            this.selectedItem = seasonCollection.Where(c => c.IsCurrentSeason).First();
        }

        public object GetNext(object relativeTo)
        {
            return seasonCollection[(seasonCollection.IndexOf((SeasonObject)relativeTo) + 1) % 12];
        }

        public object GetPrevious(object relativeTo)
        {
            return seasonCollection[(seasonCollection.IndexOf((SeasonObject)relativeTo) + 11) % 12];
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
