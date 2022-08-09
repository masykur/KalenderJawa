using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using KalenderJawa.LocalizedResources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;

namespace KalenderJawa
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            BuildApplicationBar();
            BuildDays();
            this.LayoutRoot.DataContext = DateTime.Today;
            // Set the data context of the listbox control to the sample data
            //DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            this.OrientationChanged += PhoneApplicationPage_OrientationChanged;
            //this.CalendarControl.DayTap += new KalenderJawa.CalendarControl.DayTapEventHandler(CalendarControl_DayTap);

            this.transformGroup = new TransformGroup();
            this.translation = new TranslateTransform();
            this.transformGroup.Children.Add(this.translation);
            ContentPanel.RenderTransform = this.transformGroup;

            this.storybaord = new Storyboard();
            this.storybaord.BeginTime = new TimeSpan(0);
            if (this.storybaord.Children.Count == 0)
            {
                this.storybaord.Children.Add(CreateTranslateAnimation());
            }
            this.storybaord.Completed += storybaord_Completed;

            this.restoreStorybaord = new Storyboard();
            this.restoreStorybaord.BeginTime = new TimeSpan(0);
            if (this.restoreStorybaord.Children.Count == 0)
            {
                this.restoreStorybaord.Children.Add(CreateTranslateAnimation());
            }
            this.ContentPanel.ManipulationDelta += ContentPanel_ManipulationDelta;
            this.ContentPanel.ManipulationCompleted += ContentPanel_ManipulationCompleted;
            this.MonthYearPicker.ValueChanged += MonthYearPicker_ValueChanged;
            UpdateTileData();
        }

        private void UpdateTileData()
        {
            JavaCalendar javaCalendar = new JavaCalendar();
            var javaDate = javaCalendar.GetJavaDate(DateTime.Today);
            CreateBackgroundTile(javaDate);
            StandardTileData LiveTile = new StandardTileData
            {
                BackgroundImage = new Uri("isostore:/Shared/ShellContent/background.jpg", UriKind.Absolute), 
                Title = "Kalender Jawa",
                BackBackgroundImage = new Uri("isostore:/Shared/ShellContent/background.jpg", UriKind.Absolute),
                BackContent = "",
                BackTitle = "Kalender Jawa"
            };
            ShellTile tile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("DefaultTitle=" + LiveTile.Title));

            if (tile != null)
            {
                tile.Update(LiveTile);
            }
        }

        void MonthYearPicker_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            UpdateCalendar();
        }
 
        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
        }
        // Helper function to build a localized ApplicationBar
        private void BuildApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton todayAppBarButton = new ApplicationBarIconButton(new Uri(string.Format(CultureInfo.CurrentCulture, "/Images/ApplicationBar.number-{0}.png", DateTime.Today.Day), UriKind.Relative)) { Text = AppResources.Today };
            todayAppBarButton.Click += delegate
            {
                MonthYearPicker.SetValue(DateTimePickerBase.ValueProperty, DateTime.Today);
            };
            ApplicationBarIconButton pinAppBarButton = new ApplicationBarIconButton(new Uri("/Images/ApplicationBar.pin.png", UriKind.Relative)) { Text = AppResources.Pin };
            ShellTile Tile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("DefaultTitle=Kalender Jawa"));
            pinAppBarButton.IsEnabled = (Tile == null);
            pinAppBarButton.Click += delegate
            {
                pinAppBarButton.IsEnabled = false;
                PinToStart();
            };
            ApplicationBarIconButton helpAppBarButton = new ApplicationBarIconButton(new Uri("/Images/appbar.questionmark.rest.png", UriKind.Relative)) { Text = AppResources.Help };
            helpAppBarButton.Click += delegate
            {
                NavigationService.Navigate(new Uri("/HelpPage.xaml", UriKind.Relative));
            };            
            ApplicationBar.Buttons.Add(todayAppBarButton);
            ApplicationBar.Buttons.Add(pinAppBarButton);
            ApplicationBar.Buttons.Add(helpAppBarButton);
        }
        private void BuildDays()
        {
            var currentCulture = CultureInfo.CurrentCulture;
            if (currentCulture.TwoLetterISOLanguageName.Equals("ID", StringComparison.CurrentCultureIgnoreCase))
            {
                var dayNames = ContentPanel.Children.Where(c => c is TextBlock).Select(c => c as TextBlock).ToList();
                for (var i = 0; i < dayNames.Count; i++)
                {
                    dayNames[i].Text = currentCulture.DateTimeFormat.DayNames[i];
                }
            }
            var activeDate = MonthYearPicker.Value.HasValue ? MonthYearPicker.Value.Value : DateTime.Today;
            var firstDay = new DateTime(activeDate.Year, activeDate.Month, 1);
            firstDay = firstDay.AddDays(-1 * (int)firstDay.DayOfWeek);
            for (int i = 0; i < 42; i++)
            {
                var day = new Controls.Day();
                day.Date = firstDay.AddDays(i);
                var column = (int)day.Date.DayOfWeek;
                var row = 2 + (int)(i / 7);
                //day.Visibility = (row > 6 && day.Date.Month == activeDate.AddMonths(1).Month) ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
                day.SetValue(Grid.RowProperty, row);
                day.SetValue(Grid.ColumnProperty, column);
                ContentPanel.Children.Add(day);
                day.Tap += delegate
                {
                    NavigationService.Navigate(new Uri("/DayPage.xaml?d=" + day.Date.ToString("MM-dd-yyyy"), UriKind.Relative));
                };
                day.BorderThickness = new Thickness(0, day.Date < firstDay.AddDays(7) ? 1 : 0, day.Date.DayOfWeek == DayOfWeek.Saturday ? 0 : 1, 1);
            }
            UpdateCalendar();
        }
        private void UpdateCalendar()
        {
            var activeDate = MonthYearPicker.Value.HasValue ? MonthYearPicker.Value.Value : DateTime.Today;
            var firstDay = new DateTime(activeDate.Year, activeDate.Month, 1);
            firstDay = firstDay.AddDays(-1 * (int)firstDay.DayOfWeek);
            var dayControls = ContentPanel.Children.Where(c => c is Controls.Day);
            int i = 0;
            bool rowVisible = true;
            List<string> javaMonths = new List<string>();
            List<string> hijriMonths = new List<string>();
            foreach (Controls.Day day in dayControls)
            {
                day.Date = firstDay.AddDays(i);
                var row = (int)(i / 7);
                if (rowVisible && row == 5 && day.Date.DayOfWeek == DayOfWeek.Sunday)
                {
                    rowVisible = activeDate.AddMonths(1).Month != day.Date.Month;
                }
                day.Visibility = rowVisible ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
                if (day.Date == DateTime.Today)
                {
                    day.Background = (Brush)Application.Current.Resources["PhoneAccentBrush"];
                }
                else
                {
                    day.ClearValue(BackgroundProperty);
                }
                if (day.Date.Month != activeDate.Month)
                {
                    day.Foreground = (Brush)Application.Current.Resources["PhoneInactiveBrush"];
                }
                else
                {
                    day.ClearValue(ForegroundProperty);
                }
                javaMonths.Add(day.JavaDate.ToString("MMMM yyyy", CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
                hijriMonths.Add(day.HijriDate.ToString("MMMM yyyy", CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture) + " هـ");
                i++;
            }
            JavaMonths.Text = string.Join(", ", javaMonths.Distinct().ToArray()) + " java";
            HijriMonths.Text = string.Join(", ", hijriMonths.Distinct().ToArray());
        }

        private void PhoneApplicationPage_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            if ((e.Orientation & PageOrientation.Portrait) == PageOrientation.Portrait)
            {
                ApplicationTitle.Visibility = System.Windows.Visibility.Visible;
                //MonthYearPicker.ClearValue(FontSizeProperty);
                ContentPanel.Margin = new Thickness(0, 0, 0, 80);
                AdControl.Visibility = Visibility.Visible;
            }
            else
            {
                ApplicationTitle.Visibility = System.Windows.Visibility.Collapsed;
                //MonthYearPicker.SetValue(FontSizeProperty, 24);
                ContentPanel.Margin = new Thickness(0, 0, 0, 0);
                AdControl.Visibility = Visibility.Collapsed;
            }
        }
        
        void storybaord_Completed(object sender, EventArgs e)
        {
            if (this.translation.X != 0)
            {
                var animation = this.restoreStorybaord.Children[0];
                animation.SetValue(DoubleAnimation.FromProperty, -1 * this.translation.X);
                animation.SetValue(DoubleAnimation.ToProperty, 0d);
                DateTime targetDate;
                if (MonthYearPicker.Value.HasValue)
                {
                    targetDate = MonthYearPicker.Value.Value.AddMonths(-1 * Math.Sign(this.translation.X));
                }
                else
                {
                    targetDate = DateTime.Today.AddMonths(-1 * Math.Sign(this.translation.X));
                }
                MonthYearPicker.SetValue(DateTimePickerBase.ValueProperty, targetDate);
                //UpdateCalendar();
                //this.CalendarControl.DataContext = MonthYearPicker.Value.Value;

                this.restoreStorybaord.Begin();
            }
            //this.translation.X = 0;
        }
        
        private Timeline CreateTranslateAnimation()
        {
            DoubleAnimation animation = new DoubleAnimation();
            Storyboard.SetTargetProperty(animation, new PropertyPath("X"));
            Storyboard.SetTarget(animation, this.translation);
            animation.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 300));
            animation.To = 0;
            return animation;
        }
        private TransformGroup transformGroup;
        private TranslateTransform translation;
        private Storyboard storybaord;
        private Storyboard restoreStorybaord;

        private void ContentPanel_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            var animation = this.storybaord.Children[0];
            animation.SetValue(DoubleAnimation.FromProperty, e.TotalManipulation.Translation.X);
            if (Math.Abs(e.TotalManipulation.Translation.X) > 50)
            {
                animation.SetValue(DoubleAnimation.ToProperty, (double)Math.Sign(e.TotalManipulation.Translation.X) * 480);
            }
            else
            {
                animation.SetValue(DoubleAnimation.ToProperty, 0d);
            }
            this.storybaord.Begin();
            //this.translation.Y = 0;

        }

        private void ContentPanel_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            this.translation.X += e.DeltaManipulation.Translation.X;
        }

        private void PinToStart()
        {
            StandardTileData LiveTile = new StandardTileData
            {
                BackgroundImage = new Uri("isostore:/Shared/ShellContent/background.jpg", UriKind.Absolute), //new Uri("/Images/calendar-173-tile.png", UriKind.Relative),
                Title = "Kalender Jawa",
                BackBackgroundImage = new Uri("isostore:/Shared/ShellContent/background.jpg", UriKind.Absolute) //new Uri("/Images/calendar-173-tile.png", UriKind.Relative)
            };
            ShellTile Tile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("DefaultTitle=" + LiveTile.Title));
            if (Tile == null)
            {
                try
                {
                    ShellTile.Create(new Uri("/MainPage.xaml?DefaultTitle=" + LiveTile.Title, UriKind.Relative), LiveTile);
                }
                catch (Exception)
                {
                    MessageBox.Show("I prefer not to be pinned");
                }
            }
            else
            {
                MessageBox.Show("The tile is already pinned");
            }
        }

        private void CreateBackgroundTile(JavaDate javaDate)
        {
            string fileName = @"\Shared\ShellContent\background.jpg";
            Controls.TileControl frontTile = new Controls.TileControl();
            Data.TileData tileData = new Data.TileData { DayName = javaDate.Pasaran.ToString().ToLower(CultureInfo.CurrentCulture), DayOfMonth = javaDate.Day };
            frontTile.DataContext = tileData;
            frontTile.Background = new SolidColorBrush(Colors.Orange);

            frontTile.Measure(new Size(173, 173));
            frontTile.Arrange(new Rect(0, 0, 173, 173));
            WriteableBitmap bitmap = new WriteableBitmap(173, 173);
            bitmap.Render(frontTile, null);
            bitmap.Invalidate();

            using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isolatedStorage.FileExists(fileName))
                    isolatedStorage.DeleteFile(fileName);

                IsolatedStorageFileStream fileStream = isolatedStorage.CreateFile(fileName);
                bitmap.SaveJpeg(fileStream, 173, 173, 0, 100);
                fileStream.Close();
            }            
        }
        public static void SaveImage(Stream imageStream, string fileName, int orientation, int quality)
        {
            using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isolatedStorage.FileExists(fileName))
                    isolatedStorage.DeleteFile(fileName);

                IsolatedStorageFileStream fileStream = isolatedStorage.CreateFile(fileName);
                BitmapImage bitmap = new BitmapImage();
                bitmap.SetSource(imageStream);

                WriteableBitmap wb = new WriteableBitmap(bitmap);
                wb.SaveJpeg(fileStream, wb.PixelWidth, wb.PixelHeight, orientation, quality);
                fileStream.Close();
            }
        }
    }
}