
using System;
namespace KalenderJawa
{
    public class MonthLoopingDataSource : LoopingDataSourceBase
    {
        public MonthLoopingDataSource()
        {
            var month = new Month { MonthNumber = DateTime.Today.Month };
            if (this.SelectedItem == null)
            {
                this.SelectedItem = month; 
            }
        }
        public override object GetNext(object relativeTo)
        {
            int nextValue = ((Month)relativeTo).MonthNumber + 1;
            if (nextValue > 12)
            {
                nextValue = 1;
            }
            return new Month { MonthNumber = nextValue };
        }

        public override object GetPrevious(object relativeTo)
        {
            int prevValue = ((Month)relativeTo).MonthNumber - 1;
            if (prevValue < 1) 
            { 
                prevValue = 12; 
            }
            return new Month { MonthNumber = prevValue };
        }
    }
}
