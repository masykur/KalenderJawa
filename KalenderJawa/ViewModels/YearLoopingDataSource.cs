
using System;
namespace KalenderJawa
{
    public class YearLoopingDataSource : LoopingDataSourceBase
    {
        private int minValue;
        private int maxValue;
        public YearLoopingDataSource()
        {
            this.MaxValue = DateTime.Today.AddYears(10).Year;
            this.MinValue = DateTime.Today.AddYears(-10).Year;
            this.SelectedItem = new Year { YearNumber = DateTime.Today.Year }; 
        }
        public override object GetNext(object relativeTo)
        {
            int nextValue = ((Year)relativeTo).YearNumber + 1;
            if (nextValue > this.maxValue)
            {
                nextValue = this.minValue;
            }
            return new Year { YearNumber = nextValue };
        }

        public override object GetPrevious(object relativeTo)
        {
            int prevValue = ((Year)relativeTo).YearNumber - 1;
            if (prevValue < this.minValue)
            {
                prevValue = this.maxValue;
            }
            return new Year { YearNumber = prevValue };
        }

        public int MinValue
        {
            get
            {
                return this.minValue;
            }
            set
            {
                if (value >= this.MaxValue)
                {
                    throw new ArgumentOutOfRangeException("MinValue", "MinValue cannot be equal or greater than MaxValue");
                }
                this.minValue = value;
            }
        }

        public int MaxValue
        {
            get
            {
                return this.maxValue;
            }
            set
            {
                if (value <= this.MinValue)
                {
                    throw new ArgumentOutOfRangeException("MaxValue", "MaxValue cannot be equal or lower than MinValue");
                }
                this.maxValue = value;
            }
        }
    }
}
