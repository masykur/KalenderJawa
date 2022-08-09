using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Primitives;
using System.Windows.Navigation;
using System.Globalization;
using Microsoft.Phone.Shell;

namespace KalenderJawa
{
    public partial class MonthYearPickerPage : PhoneApplicationPage, IDateTimePickerPage
    {
        YearLoopingDataSource yearDataSource;
        MonthLoopingDataSource monthDataSource;
        public MonthYearPickerPage()
        {
            InitializeComponent();
            yearDataSource = new YearLoopingDataSource { MinValue = 1900, MaxValue = 2099 };
            if (Value.HasValue)
            {
                yearDataSource.SelectedItem = Value.Value.Year;
            }
            monthDataSource = new MonthLoopingDataSource();
            if (Value.HasValue)
            {
                monthDataSource.SelectedItem = Value.Value.Month;
            }
            PrimarySelector.DataSource = yearDataSource;
            SecondarySelector.DataSource = monthDataSource;
            //TertiarySelector.DataSource = new YearLoopingDataSource();

            InitializeDateTimePickerPage(PrimarySelector, SecondarySelector);
        }

        private LoopingSelector _primarySelectorPart;
        private LoopingSelector _secondarySelectorPart;
        /// <summary>
        /// Initializes the DateTimePickerPageBase class; must be called from the subclass's constructor.
        /// </summary>
        /// <param name="primarySelector">Primary selector.</param>
        /// <param name="secondarySelector">Secondary selector.</param>
        /// <param name="tertiarySelector">Tertiary selector.</param>
        protected void InitializeDateTimePickerPage(LoopingSelector primarySelector, LoopingSelector secondarySelector)
        {
            if (null == primarySelector)
            {
                throw new ArgumentNullException("primarySelector");
            }
            if (null == secondarySelector)
            {
                throw new ArgumentNullException("secondarySelector");
            }

            _primarySelectorPart = primarySelector;
            _secondarySelectorPart = secondarySelector;
            //// Hook up to interesting events
            //_primarySelectorPart.DataSource.SelectionChanged += OnDataSourceSelectionChanged;
            //_secondarySelectorPart.DataSource.SelectionChanged += OnDataSourceSelectionChanged;
            //_primarySelectorPart.IsExpandedChanged += OnSelectorIsExpandedChanged;
            //_secondarySelectorPart.IsExpandedChanged += OnSelectorIsExpandedChanged;

            // Hide all selectors
            //_primarySelectorPart.Visibility = Visibility.Collapsed;
            //_secondarySelectorPart.Visibility = Visibility.Collapsed;

            //// Position and reveal the culture-relevant selectors
            //int column = 0;
            //foreach (LoopingSelector selector in GetSelectorsOrderedByCulturePattern())
            //{
            //    Grid.SetColumn(selector, column);
            //    selector.Visibility = Visibility.Visible;
            //    column++;
            //}

            // Hook up to storyboard(s)
            //FrameworkElement templateRoot = VisualTreeHelper.GetChild(this, 0) as FrameworkElement;
            //if (null != templateRoot)
            //{
            //    foreach (VisualStateGroup group in VisualStateManager.GetVisualStateGroups(templateRoot))
            //    {
            //        if (VisibilityGroupName == group.Name)
            //        {
            //            foreach (VisualState state in group.States)
            //            {
            //                if ((ClosedVisibilityStateName == state.Name) && (null != state.Storyboard))
            //                {
            //                    _closedStoryboard = state.Storyboard;
            //                    _closedStoryboard.Completed += OnClosedStoryboardCompleted;
            //                }
            //            }
            //        }
            //    }
            //}

            // Customize the ApplicationBar Buttons by providing the right text
            if (null != ApplicationBar)
            {
                foreach (object obj in ApplicationBar.Buttons)
                {
                    IApplicationBarIconButton button = obj as IApplicationBarIconButton;
                    if (null != button)
                    {
                        if ("DONE" == button.Text)
                        {
                            button.Text = Microsoft.Phone.Controls.LocalizedResources.ControlResources.DateTimePickerDoneText;
                            button.Click += OnDoneButtonClick;
                        }
                        else if ("CANCEL" == button.Text)
                        {
                            button.Text = Microsoft.Phone.Controls.LocalizedResources.ControlResources.DateTimePickerCancelText;
                            button.Click += OnCancelButtonClick;
                        }
                    }
                }
            }

            // Play the Open state
            //VisualStateManager.GoToState(this, OpenVisibilityStateName, true);
        }

        private void OnDoneButtonClick(object sender, EventArgs e)
        {
            // Commit the value and close
            //Debug.Assert((_primarySelectorPart.DataSource.SelectedItem == _secondarySelectorPart.DataSource.SelectedItem) && (_secondarySelectorPart.DataSource.SelectedItem == _tertiarySelectorPart.DataSource.SelectedItem));
            Value = new DateTime(((Year)PrimarySelector.DataSource.SelectedItem).YearNumber, ((Month)SecondarySelector.DataSource.SelectedItem).MonthNumber, 1);
            ClosePickerPage();
        }

        private void OnCancelButtonClick(object sender, EventArgs e)
        {
            // Close without committing a value
            Value = null;
            ClosePickerPage();
        }

        private void ClosePickerPage()
        {
            NavigationService.GoBack();
            //// Play the Close state (if available)
            //if (null != _closedStoryboard)
            //{
            //    VisualStateManager.GoToState(this, ClosedVisibilityStateName, true);
            //}
            //else
            //{
            //    OnClosedStoryboardCompleted(null, null);
            //}
        }

        private void OnClosedStoryboardCompleted(object sender, EventArgs e)
        {
            // Close the picker page
            NavigationService.GoBack();
        }

        /// <summary>
        /// Event that is invoked when the Value property changes.
        /// </summary>
        public event EventHandler<DateTimeValueChangedEventArgs> ValueChanged;

        public DateTime? Value
        {
            get { return (DateTime?)GetValue(ValueProperty); }
            set 
            {
                if (value.HasValue) 
                {
                    monthDataSource.SelectedItem = new Month { MonthNumber = value.Value.Month };
                    yearDataSource.SelectedItem = new Year { YearNumber = value.Value.Year };
                }
                SetValue(ValueProperty, value); 
            }
        }
        /// <summary>
        /// Identifies the Value DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(DateTime?), typeof(IDateTimePickerPage), new PropertyMetadata(null, OnValueChanged));

        private static void OnValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {

            ((MonthYearPickerPage)o).OnValueChanged((DateTime?)e.OldValue, (DateTime?)e.NewValue);
        }

        private void OnValueChanged(DateTime? oldValue, DateTime? newValue)
        {
            UpdateValueString();
            OnValueChanged(new DateTimeValueChangedEventArgs(oldValue, newValue));
        }

        /// <summary>
        /// Called when the value changes.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected virtual void OnValueChanged(DateTimeValueChangedEventArgs e)
        {
            var handler = ValueChanged;
            if (null != handler)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Gets the string representation of the selected value.
        /// </summary>
        public string ValueString
        {
            get { return ((string)GetValue(ValueStringProperty)).ToLower(CultureInfo.CurrentCulture); }
            private set { SetValue(ValueStringProperty, value); }
        }

        /// <summary>
        /// Identifies the ValueString DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty ValueStringProperty = DependencyProperty.Register(
            "ValueString", typeof(string), typeof(IDateTimePickerPage), null);


        private void OnValueStringFormatChanged(/*string oldValue, string newValue*/)
        {
            UpdateValueString();
        }

        private void UpdateValueString()
        {
            ValueString = string.Format(CultureInfo.CurrentCulture, "{0:MMMM yyyy}", Value);// string.Format(CultureInfo.CurrentCulture, ValueStringFormat ?? ValueStringFormatFallback, Value);
        }
    }
}