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
using System.Globalization;
using Microsoft.Phone.Shell;
using KalenderJawa.LocalizedResources;

namespace KalenderJawa
{
    public partial class DayPage : PhoneApplicationPage
    {
        private static KalenderJawa.Data.CalendarDataSource calendarDataSource = new KalenderJawa.Data.CalendarDataSource();
        private static KalenderJawa.Data.SeasonDataSource seasonDataSource = new KalenderJawa.Data.SeasonDataSource();
        public DayPage()
        {
            InitializeComponent();

            calendarDataSource.SelectionChanged += new EventHandler<SelectionChangedEventArgs>(calendarDataSource_SelectionChanged);
            Calendars.DataSource = calendarDataSource;

            Seasons.DataSource = seasonDataSource;

            BuildApplicationBar();
        }

        void calendarDataSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem =  e.AddedItems[0] as Calendar;
            if (selectedItem != null)
            {
                if (selectedItem.GregorianDate == DateTime.Today)
                {
                    PivotDaily.Title = "TODAY";
                }
                else
                {
                    PivotDaily.Title = selectedItem.GregorianDate.ToString("dddd, d MMMM yyyy", CultureInfo.CurrentCulture).ToUpper(CultureInfo.CurrentCulture);
                }
            }
        }

        // Helper function to build a localized ApplicationBar
        private ApplicationBarIconButton todayAppBarButton = new ApplicationBarIconButton(new Uri(string.Format(CultureInfo.CurrentCulture, "/Images/ApplicationBar.number-{0}.png", DateTime.Today.Day), UriKind.Relative)) { Text = AppResources.Today };
        private void BuildApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            todayAppBarButton.Click += delegate
            {
                calendarDataSource.SelectedItem = new Calendar(DateTime.Today); 
            };
            ApplicationBarIconButton helpAppBarButton = new ApplicationBarIconButton(new Uri("/Images/appbar.questionmark.rest.png", UriKind.Relative)) { Text = AppResources.Help };
            helpAppBarButton.Click += delegate
            {
                NavigationService.Navigate(new Uri("/HelpPage.xaml", UriKind.Relative));
            };
            ApplicationBar.Buttons.Add(helpAppBarButton);
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string dateString;
            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New && NavigationContext.QueryString.TryGetValue("d", out dateString))
            {
                var date = DateTime.Parse(dateString, new CultureInfo("en-US"));
                calendarDataSource.SelectedItem = new Calendar(date);
            }
        }

        private void PivotDaily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((PivotItem)e.AddedItems[0]).Header.Equals("day"))
            {
                ApplicationBar.Buttons.Insert(0, todayAppBarButton);
            }
            else
            {
                ApplicationBar.Buttons.Remove(todayAppBarButton);
            }
        }
    }
}